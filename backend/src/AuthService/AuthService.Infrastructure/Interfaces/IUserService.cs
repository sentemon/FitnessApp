using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(string id);
    Task<User> UpdateUserAsync(string id, string? firstName, string? lastName, string? username, string? email);
}