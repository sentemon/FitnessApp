using System.Net.Http.Json;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly KeycloakConfig _keycloakConfig;

    public UserService(IHttpClientFactory httpClientFactory, ITokenService tokenService,
        IOptions<KeycloakConfig> keycloakConfig)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _tokenService = tokenService;
        _keycloakConfig = keycloakConfig.Value;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var accessToken = await _tokenService.GetAdminAccessTokenAsync();

        _tokenService.SetAccessToken(accessToken);

        var response = await _httpClient.GetAsync($"admin/realms/{_keycloakConfig.Realm}/users/{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<User>();
    }

    public async Task<User> UpdateAsync(string id, string? firstName, string? lastName, string? username, string? email)
    {
        var adminAccessToken = await _tokenService.GetAdminAccessTokenAsync();

        _tokenService.SetAccessToken(adminAccessToken);

        var updateKeycloakUser = new
        {
            firstName,
            lastName,
            username,
            email
        };

        var response =
            await _httpClient.PutAsJsonAsync($"admin/realms/{_keycloakConfig.Realm}/users/{id}", updateKeycloakUser);
        response.EnsureSuccessStatusCode();

        var user = await GetByIdAsync(id);

        if (user == null)
        {
            throw new Exception("User not found in keycloak DB.");
        }

        return user;
    }

    public async Task<bool> ResetPasswordAsync(string id, string newPassword)
    {
        var adminAccessToken = await _tokenService.GetAdminAccessTokenAsync();

        _tokenService.SetAccessToken(adminAccessToken);

        var credentials = new
        {
            type = "password",
            value = newPassword,
            temporary = false
        };

        var response = await _httpClient.PutAsJsonAsync($"admin/realms/{_keycloakConfig.Realm}/users/{id}/reset-password", credentials);
        response.EnsureSuccessStatusCode();

        return response.IsSuccessStatusCode;
    }
}