using System.Net.Http.Headers;
using System.Net.Http.Json;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Newtonsoft.Json;

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

    public async Task<User?> GetUserByIdAsync(string externalUserId)
    {
        try
        {
            var accessToken = await GetAdminAccessTokenAsync();
            
            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken), "Failed to obtain access token.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            var response = await _httpClient.GetAsync($"admin/realms/{_realm}/users/{externalUserId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving user: {ex.Message}");
            return null;
        }
    }

    public async Task<User?> RegisterAsync(string firstName, string lastName, string username, string email, string password)
    {
        try
        {
            var accessToken = await GetAdminAccessTokenAsync(); 
            
            if (accessToken == null) 
            {
                Console.WriteLine("Failed to obtain access token."); 
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to create user in Keycloak. StatusCode: {response.StatusCode}");
                return null;
            }

            if (response.Headers.Location != null)
            {
                var userId = response.Headers.Location.Segments.Last();
                return await GetUserByIdAsync(userId);
            }

            Console.WriteLine("User creation succeeded, but no Location header returned.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while creating user: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> LoginAsync(string username, string password)
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
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Login failed. StatusCode: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(content);

            return tokenResponse?.AccessToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            return null;
        }
    }

    public void SetAccessToken(string accessToken)
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
            var tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(content);

            return tokenResponse?.AccessToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving access token: {ex.Message}");
            return null;
        }
    }
}