using PostService.Domain.Enums;

namespace PostService.Domain.Entities;

public class Post
{
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ContentUrl { get; private set; }
    public ContentType ContentType { get; private set; }
    public int LikeCount { get; private set; }
    public int CommentCount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Post(Guid postId, Guid userId, string title, string description, string contentUrl, ContentType contentType)
    {
        PostId = postId;
        UserId = userId;
        Title = title;
        Description = description;
        ContentUrl = contentUrl;
        ContentType = contentType;
        LikeCount = 0;
        CommentCount = 0;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void IncrementCommentCount() => CommentCount++;
    
    public void IncrementLikeCount() => LikeCount++;
}
