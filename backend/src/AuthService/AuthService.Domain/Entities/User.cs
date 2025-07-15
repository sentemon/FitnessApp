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

    private readonly List<Follow> _followers = [];
    public IReadOnlyCollection<Follow> Followers => _followers.AsReadOnly();

    private readonly List<Follow> _followings = [];
    public IReadOnlyCollection<Follow> Followings => _followings.AsReadOnly();

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

    public void FollowUser(User targetUser)
    {
        if (targetUser.Id == Id)
            throw new InvalidOperationException("Cannot follow yourself.");
        if (_followings.Any(f => f.FollowingId == targetUser.Id))
            return;

        var follow = Follow.Create(Id, targetUser.Id);
        _followings.Add(follow);
        targetUser._followers.Add(follow);
    }

    public void UnfollowUser(User targetUser)
    {
        if (targetUser.Id == Id)
            throw new InvalidOperationException("Cannot unfollow yourself.");
        
        var follow = _followings.FirstOrDefault(f => f.FollowingId == targetUser.Id);
        if (follow is null)
            return;
        
        _followings.Remove(follow);
        targetUser._followers.Remove(follow);
    }
    
    #pragma warning disable CS8618
    // Required by EF Core
    private User() { }
    #pragma warning restore CS8618
}