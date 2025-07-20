namespace AuthService.Application.DTOs;

public record UserDto(
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string? ImageUrl,
    uint FollowingCount,
    uint FollowersCount
);