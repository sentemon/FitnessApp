using AuthService.Domain.Entities;
using AuthService.Infrastructure.Models;

namespace AuthService.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(string firstName, string lastName, string username, string email, string password);
    Task<KeycloakTokenResponse> LoginAsync(string username, string password);
    Task<bool> LogoutAsync(string refreshToken);
}