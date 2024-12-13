using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;

namespace AuthService.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly string _realm;
    private readonly string _adminUsername;
    private readonly string _adminPassword;

    public KeycloakService(HttpClient httpClient, string realm, string adminUsername, string adminPassword)
    {
        _httpClient = httpClient;
        _realm = realm;
        _adminUsername = adminUsername;
        _adminPassword = adminPassword;
    }

    public async Task<User> GetUserAsync(string externalUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<string> RegisterAsync(string username, string email, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public void SetAccessToken(string accessToken)
    {
        throw new NotImplementedException();
    }
}