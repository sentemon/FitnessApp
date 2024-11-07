namespace PostService.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Comment(Guid postId, Guid userId, string content)
    {
        if (postId == Guid.Empty)
        {
            throw new ArgumentException("PostId cannot be empty.", nameof(userId));
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }
        
        Id = Guid.NewGuid();
        PostId = postId;
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}