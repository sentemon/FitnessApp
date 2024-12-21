namespace AuthService.Infrastructure.Interfaces;

public interface ITokenService
{
    Task<string?> GetAdminAccessTokenAsync();
}