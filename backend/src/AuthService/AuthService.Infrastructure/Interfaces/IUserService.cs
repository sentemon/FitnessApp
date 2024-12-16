using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Interfaces;

public interface IUserService
{
    Task<User?> GetByIdAsync(string id);
    Task<User> UpdateAsync(string id, string? firstName, string? lastName, string? username, string? email);
    Task<bool> ResetPasswordAsync(string id, string newPassword);
}