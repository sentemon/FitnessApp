namespace ChatService.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<UserChat> UserChats { get; private set; } = [];
    public ICollection<Message> Messages { get; private set; } = [];

    private User(string id, string firstName, string lastName, string username, string email, string imageUrl)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
        ImageUrl = imageUrl;
    }

    public static User Create(string id, string firstName, string lastName, string username, string email, string imageUrl)
    {
        return new User(id, firstName, lastName, username, email, imageUrl);
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}