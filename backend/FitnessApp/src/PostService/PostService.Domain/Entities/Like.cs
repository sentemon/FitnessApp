namespace PostService.Domain.Entities;

public class Like
{
    public Guid LikeId { get; private set; }
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Like(Guid likeId, Guid postId, Guid userId)
    {
        LikeId = likeId;
        PostId = postId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
}