namespace PostService.Domain.Entities;

public class Like
{
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Like(Guid id, Guid postId, Guid userId)
    {
        Id = id;
        PostId = postId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
}