using System.Net.Http.Headers;
using System.Net.Http.Json;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using System.Text.Json;

namespace AuthService.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly string _realm;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _adminUsername;
    private readonly string _adminPassword;

    public KeycloakService(HttpClient httpClient, string realm, string clientId, string clientSecret, string adminUsername, string adminPassword)
    {
        _httpClient = httpClient;
        _realm = realm;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _adminUsername = adminUsername;
        _adminPassword = adminPassword;
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        try
        {
            var accessToken = await GetAdminAccessTokenAsync();

            SetAccessToken(accessToken);
            
            var response = await _httpClient.GetAsync($"admin/realms/{_realm}/users/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while retrieving user: {ex.Message}");
        }
    }

    public async Task<User> RegisterAsync(string firstName, string lastName, string username, string email, string password)
    {
        try
        {
            var accessToken = await GetAdminAccessTokenAsync();
            
            SetAccessToken(accessToken);

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
            var user = await GetUserByIdAsync(userId);

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

    private void SetAccessToken(string? accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty.", nameof(accessToken));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private async Task<string?> GetAdminAccessTokenAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"realms/master/protocol/openid-connect/token")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "admin-cli"),
                    new KeyValuePair<string, string>("username", _adminUsername),
                    new KeyValuePair<string, string>("password", _adminPassword)
                })
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(content);

            return tokenResponse?.AccessToken;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while retrieving access token: {ex.Message}");
        }
    }
}