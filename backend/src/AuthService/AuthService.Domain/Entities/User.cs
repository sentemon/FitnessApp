using AuthService.Domain.ValueObjects;

namespace AuthService.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public bool EmailVerified { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    private User(string id, string firstName, string lastName, Username username, Email email, string? imageUrl = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
        EmailVerified = false;
        ImageUrl = imageUrl ?? string.Empty;
    }
    
    public static User Create(string id, string firstName, string lastName, string username, string email, string? imageUrl = null)
    {
        return new User(
            id: id,
            firstName: firstName,
            lastName: lastName,
            username: Username.Create(username),
            email: Email.Create(email),
            imageUrl: imageUrl
        );
    }

    public void Update(string? firstName, string? lastName, string? username, string? email, string? imageUrl = null)
    {
        FirstName = firstName ?? FirstName;
        LastName = lastName ?? LastName;
        Username = username != null ? Username.Create(username) : Username;
        Email = email != null ? Email.Create(email) : Email;
        ImageUrl = imageUrl ?? ImageUrl;
    }

    public void VerifyEmail()
    {
        EmailVerified = true;
    }
}