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

    public UserService(IHttpClientFactory httpClientFactory, ITokenService tokenService, IOptions<KeycloakConfig> keycloakConfig)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _tokenService = tokenService;
        _keycloakConfig = keycloakConfig.Value;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        try
        {
            var accessToken = await _tokenService.GetAdminAccessTokenAsync();

            _tokenService.SetAccessToken(accessToken);
            
            var response = await _httpClient.GetAsync($"admin/realms/{_keycloakConfig.Realm}/users/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while retrieving user: {ex.Message}");
        }
    }
    
    public async Task<User> UpdateAsync(string id, string? firstName, string? lastName, string? username, string? email)
    {
        try
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

            var response = await _httpClient.PutAsJsonAsync($"admin/realms/{_keycloakConfig.Realm}/users/{id}", updateKeycloakUser);
            response.EnsureSuccessStatusCode();

            var user = await GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found in keycloak DB.");
            }
            
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}