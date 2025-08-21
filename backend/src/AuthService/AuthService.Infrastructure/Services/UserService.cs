using System.Net.Http.Headers;
using System.Net.Http.Json;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;

namespace AuthService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly KeycloakConfig _keycloakConfig;

    public UserService(IHttpClientFactory httpClientFactory, ITokenService tokenService, KeycloakConfig keycloakConfig)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _tokenService = tokenService;
        _keycloakConfig = keycloakConfig;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var accessToken = await _tokenService.GetAdminAccessTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync($"admin/realms/{_keycloakConfig.Realm}/users/{id}");
        response.EnsureSuccessStatusCode();

        var keycloakUser = await response.Content.ReadFromJsonAsync<KeycloakUser>();

        if (keycloakUser == null)
        {
            return null;
        }

        var user = User.Create(
            keycloakUser.Id,
            keycloakUser.FirstName,
            keycloakUser.LastName,
            keycloakUser.Username,
            keycloakUser.Email
        );

        return user;
    }

    public async Task<User> UpdateAsync(string id, string? firstName, string? lastName, string? username, string? email)
    {
        var adminAccessToken = await _tokenService.GetAdminAccessTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminAccessToken);

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
    
    public async Task<bool> DeleteAsync(string id)
    {
        var adminAccessToken = await _tokenService.GetAdminAccessTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminAccessToken);

        var request = new HttpRequestMessage(HttpMethod.Delete, $"admin/realms/{_keycloakConfig.Realm}/users/{id}");

        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ResetPasswordAsync(string id, string newPassword)
    {
        var adminAccessToken = await _tokenService.GetAdminAccessTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminAccessToken);

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