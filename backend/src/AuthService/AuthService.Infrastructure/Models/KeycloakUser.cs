namespace AuthService.Infrastructure.Models;

public record KeycloakUser(
    string Id,
    string Username,
    string FirstName,
    string LastName,
    string Email
);