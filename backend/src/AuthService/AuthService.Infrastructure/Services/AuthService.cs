using System.Net.Http.Json;
using System.Text.Json;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;

namespace AuthService.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly string _realm;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public AuthService(IHttpClientFactory httpClientFactory, ITokenService tokenService, IUserService userService, string realm, string clientId, string clientSecret)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _tokenService = tokenService;
        _userService = userService;
        
        _realm = realm;
        _clientId = clientId;
        _clientSecret = clientSecret;
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

            var response = await _httpClient.PostAsJsonAsync($"admin/realms/{_realm}/users", keycloakUser);
            response.EnsureSuccessStatusCode();

            if (response.Headers.Location == null)
            {
                throw new Exception("User creation succeeded, but no Location header returned.");
            }

            var userId = response.Headers.Location.Segments.Last(); 
            var user = await _userService.GetUserByIdAsync(userId);

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
            var request = new HttpRequestMessage(HttpMethod.Post, $"realms/{_realm}/protocol/openid-connect/token")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("client_secret", _clientSecret),
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
            var request = new HttpRequestMessage(HttpMethod.Post, $"realms/{_realm}/protocol/openid-connect/logout")
            {
                Content = new FormUrlEncodedContent(new []
                {
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string,string>("client_secret", _clientSecret),
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
}