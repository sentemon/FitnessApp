using System.Net.Http.Headers;
using System.Text.Json;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakConfig _keycloakConfig;

    public TokenService(IHttpClientFactory httpClientFactory, IOptions<KeycloakConfig> keycloakConfig)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _keycloakConfig = keycloakConfig.Value;
    }

    public void SetAccessToken(string? accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty.", nameof(accessToken));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    public async Task<string?> GetAdminAccessTokenAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"realms/master/protocol/openid-connect/token")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "admin-cli"),
                    new KeyValuePair<string, string>("username", _keycloakConfig.AdminUsername),
                    new KeyValuePair<string, string>("password", _keycloakConfig.AdminPassword)
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