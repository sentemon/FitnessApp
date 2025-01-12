namespace Shared.DTO.Messages;

public record UserCreatedEventMessage(
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string ImageUrl,
    DateTime CreatedAt
);