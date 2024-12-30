namespace PostService.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Username { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(string id, string firstName, string lastName, string username, string imageUrl, DateTime createdAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        ImageUrl = imageUrl;
        CreatedAt = createdAt;
    }
}