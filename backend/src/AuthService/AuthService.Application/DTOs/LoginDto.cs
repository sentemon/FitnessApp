namespace AuthService.Application.DTOs;

public record LoginDto(
    string Username,
    string Password
);