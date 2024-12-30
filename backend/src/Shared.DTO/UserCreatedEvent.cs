namespace Shared.DTO;

public record UserCreatedEvent(
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string ImageUrl,
    DateTime CreatedAt
);