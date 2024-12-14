using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Interfaces;

public interface IKeycloakService
{
    Task<User?> GetUserByIdAsync(string externalUserId);
    Task<User?> RegisterAsync(string firstName, string lastName, string username, string email, string password);
    Task<string?> LoginAsync(string username, string password);
    void SetAccessToken(string accessToken);
}