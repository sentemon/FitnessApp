using System.Net.Http.Json;
using System.Text.Json;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly KeycloakConfig _keycloakConfig;

    public AuthService(IHttpClientFactory httpClientFactory, ITokenService tokenService, IUserService userService, IOptions<KeycloakConfig> keycloakConfig)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _tokenService = tokenService;
        _userService = userService;
        _keycloakConfig = keycloakConfig.Value;
    }

    public async Task<User> RegisterAsync(string firstName, string lastName, string username, string email, string password)
    {
        try
        {
            var adminAccessToken = await _tokenService.GetAdminAccessTokenAsync();
            
            _tokenService.SetAccessToken(adminAccessToken);

            var keycloakUser = new
            {
                firstName,
                lastName,
                username,
                email,
                enabled = true,
                credentials = new[]
                {
                    new
                    {
                        type = "password",
                        value = password,
                        temporary = false
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync($"admin/realms/{_keycloakConfig.Realm}/users", keycloakUser);
            response.EnsureSuccessStatusCode();

            if (response.Headers.Location == null)
            {
                throw new Exception("User creation succeeded, but no Location header returned.");
            }

            var userId = response.Headers.Location.Segments.Last(); 
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception($"User with Id: {userId} not found.");
            }

            return user;

        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while creating user: {ex.Message}");
        }
    }
    
    public async Task<KeycloakTokenResponse> LoginAsync(string username, string password)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"realms/{_keycloakConfig.Realm}/protocol/openid-connect/token")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", _keycloakConfig.ClientId),
                    new KeyValuePair<string, string>("client_secret", _keycloakConfig.ClientSecret),
                    new KeyValuePair<string, string>("scope", "web-origins"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                })
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(content);

            if (tokenResponse == null)
            {
                throw new NullReferenceException();
            }

            return tokenResponse;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error during login: {ex.Message}");
        }
    }
    
    public async Task<bool> LogoutAsync(string refreshToken)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"realms/{_keycloakConfig.Realm}/protocol/openid-connect/logout")
            {
                Content = new FormUrlEncodedContent(new []
                {
                    new KeyValuePair<string, string>("client_id", _keycloakConfig.ClientId),
                    new KeyValuePair<string,string>("client_secret", _keycloakConfig.ClientSecret),
                    new KeyValuePair<string,string>("refresh_token", refreshToken)
                })
            };
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> SendVerifyEmailAsync(string userId)
    {
        try
        {
            var adminAccessToken = await _tokenService.GetAdminAccessTokenAsync();

            _tokenService.SetAccessToken(adminAccessToken);
            
            var request = new HttpRequestMessage(HttpMethod.Put,
                $"admin/realms/{_keycloakConfig.Realm}/users/{userId}/send-verify-email");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}