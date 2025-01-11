namespace Shared.DTO.Messages;

public record UserUpdatedEventMessage( 
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string ImageUrl
);