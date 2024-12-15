using System.Net.Http.Json;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;

namespace AuthService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly string _realm;

    public UserService(IHttpClientFactory httpClientFactory, ITokenService tokenService, string realm)
    {
        _httpClient = httpClientFactory.CreateClient("KeycloakClient");
        _tokenService = tokenService;
        
        _realm = realm;
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        try
        {
            var accessToken = await _tokenService.GetAdminAccessTokenAsync();

            _tokenService.SetAccessToken(accessToken);
            
            var response = await _httpClient.GetAsync($"admin/realms/{_realm}/users/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while retrieving user: {ex.Message}");
        }
    }
    
    public async Task<User> UpdateUserAsync(string id, string? firstName, string? lastName, string? username, string? email)
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

            var response = await _httpClient.PutAsJsonAsync($"admin/realms/{_realm}/users/{id}", updateKeycloakUser);
            response.EnsureSuccessStatusCode();

            var user = await GetUserByIdAsync(id);

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