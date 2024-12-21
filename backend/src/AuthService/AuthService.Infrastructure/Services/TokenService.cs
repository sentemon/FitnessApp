using System.Text.Json;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;

namespace AuthService.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakConfig _keycloakConfig;

    public TokenService(IHttpClientFactory httpClientFactory, KeycloakConfig keycloakConfig)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _keycloakConfig = keycloakConfig;
    }

    public async Task<string?> GetAdminAccessTokenAsync()
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
}