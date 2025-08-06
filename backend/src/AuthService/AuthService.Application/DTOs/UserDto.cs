namespace AuthService.Application.DTOs;

public record UserDto(
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string Email,
    DateTime LastSeenAt,
    string? ImageUrl,
    uint FollowingCount,
    uint FollowersCount
);