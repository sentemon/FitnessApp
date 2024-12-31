namespace PostService.Domain.Entities;

public class Like
{
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public string UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Like(Guid postId, string userId)
    {
        if (postId == Guid.Empty)
        {
            throw new ArgumentException("PostId cannot be empty.", nameof(postId));
        }
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }
        
        PostId = postId;
        UserId = userId;
    }
}