namespace AuthService.Domain.Entities;

public class Follow
{
    public Guid Id { get; private set; }
    public string FollowerId { get; private set; }
    public string FollowingId { get; private set; }
    public DateOnly FollowedAt { get; private set; }

    public User Follower { get; private set; }
    public User Following { get; private set; }

    public Follow(string followerId, string followingId)
    {
        FollowerId = followerId;
        FollowingId = followingId;
        FollowedAt = DateOnly.FromDateTime(DateTime.UtcNow);
    }
    
    #pragma warning disable CS8618
    // Required by EF Core
    private Follow()
    {
    }
    #pragma warning disable CS8618
    
}
