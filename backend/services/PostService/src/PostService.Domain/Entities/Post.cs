namespace PostService.Domain.Entities;

public class Post
{
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string ImageUrl { get; private set; }
    public int LikeCount { get; private set; }
    public int CommentCount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Post(Guid postId, Guid userId, string title, string content, string imageUrl)
    {
        PostId = postId;
        UserId = userId;
        Title = title;
        Content = content;
        ImageUrl = imageUrl;
        LikeCount = 0;
        CommentCount = 0;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void IncrementCommentCount() => CommentCount++;
    
    public void IncrementLikeCount() => LikeCount++;
}
