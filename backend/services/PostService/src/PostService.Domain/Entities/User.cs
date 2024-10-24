namespace PostService.Domain.Entities;

public class User
{
    public Guid UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Username { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(Guid userId, string firstName, string lastName, string username, string imageUrl)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
    }
}