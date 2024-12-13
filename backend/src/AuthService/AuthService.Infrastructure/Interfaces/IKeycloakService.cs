using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Interfaces;

public interface IKeycloakService
{
    Task<User> GetUserAsync(string externalUserId);
    Task<string> RegisterAsync(string username, string email, string password);
    Task<string> LoginAsync(string username, string password);
    void SetAccessToken(string accessToken);
}