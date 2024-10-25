namespace PostService.Domain.Entities;

public class Comment
{
    public Guid CommentId { get; private set; }
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Comment(Guid commentId, Guid postId, Guid userId, string content)
    {
        CommentId = commentId;
        PostId = postId;
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}