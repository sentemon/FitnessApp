using PostService.Domain.Enums;

namespace PostService.Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ContentUrl { get; private set; }
    public ContentType ContentType { get; private set; }
    public uint LikeCount { get; private set; }
    public uint CommentCount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Post(string userId, string title, string description, ContentType contentType)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }
        
        ValidateContent(contentType, title, description);
        
        UserId = userId;
        Title = title;
        Description = description;
        ContentUrl = string.Empty;
        ContentType = contentType;
        LikeCount = 0;
        CommentCount = 0;
    }

    public static Post CreateTextPost(string userId, string title, string description)
    {
        return new Post(userId, title, description, ContentType.Text);
    }

    public static Post CreateImagePost(string userId, string title = "", string description = "")
    {
        return new Post(userId, title, description, ContentType.Image);
    }

    public static Post CreateVideoPost(string userId, string title = "", string description = "")
    {
        return new Post(userId, title, description, ContentType.Video);
    }
    
    public void Update(string title, string description)
    {
        ValidateContent(ContentType, title, description);
        
        Title = title;
        Description = description;
    }
    
    public void IncrementCommentCount() => CommentCount++;
    public void DecrementCommentCount() => CommentCount--;
    
    public void IncrementLikeCount() => LikeCount++;
    public void DecrementLikeCount() => LikeCount--;
    
    public void SetContentUrl(string contentUrl)
    {
        if (ContentType is ContentType.Image or ContentType.Video && string.IsNullOrWhiteSpace(contentUrl))
        {
            throw new ArgumentException("ContentUrl is required for image or video content.", nameof(contentUrl));
        }
        
        ContentUrl = contentUrl;
    }
    
    private static void ValidateContent(ContentType contentType, string title, string description)
    {
        if (contentType != ContentType.Text) return;
        
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty for text content.", nameof(title));
        }
    
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be empty for text content.", nameof(description));
        }
    }
}
