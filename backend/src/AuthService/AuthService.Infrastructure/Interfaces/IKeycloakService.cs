using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Interfaces;

public interface IKeycloakService
{
    Task<User?> GetUserByIdAsync(string externalUserId);
    Task<bool> RegisterAsync(string username, string email, string password, string? accessToken);
    Task<string?> LoginAsync(string username, string password);
    void SetAccessToken(string accessToken);
}