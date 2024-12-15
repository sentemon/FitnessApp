namespace AuthService.Application.DTOs;

public record UpdateUserDto(
    string? FirstName,
    string? LastName,
    string? Username,
    string? Email
);