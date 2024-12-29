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
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Id cannot be null or empty.", nameof(id));
        }
        
        return new User(
            id,
            firstName,
            lastName,
            Username.Create(username),
            Email.Create(email),
            imageUrl
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
    
    #pragma warning disable CS8618
    // Required by EF Core
    private User() { }
    #pragma warning restore CS8618
}