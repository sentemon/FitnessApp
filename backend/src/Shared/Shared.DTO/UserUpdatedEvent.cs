namespace Shared.DTO;

public record UserUpdatedEvent( 
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string ImageUrl
);