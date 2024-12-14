namespace AuthService.Application.DTOs;

public record UserDto(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string? ImageUrl
);