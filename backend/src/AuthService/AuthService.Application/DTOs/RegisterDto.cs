namespace AuthService.Application.DTOs;

public record RegisterDto(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password
);