using PostService.Domain.Enums;

namespace PostService.Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ContentUrl { get; private set; }
    public ContentType ContentType { get; private set; }
    public uint LikeCount { get; private set; }
    public uint CommentCount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Post(Guid userId, string title, string description, string contentUrl, ContentType contentType)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }
        
        ValidateContent(contentType, title, description, contentUrl);
        
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        ContentUrl = contentUrl;
        ContentType = contentType;
        LikeCount = 0;
        CommentCount = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public static Post CreateTextPost(Guid userId, string title, string description)
    {
        return new Post(userId, title, description, string.Empty, ContentType.Text);
    }

    public static Post CreateImagePost(Guid userId, string contentUrl, string title = "", string description = "")
    {
        return new Post(userId, title, description, contentUrl, ContentType.Image);
    }

    public static Post CreateVideoPost(Guid userId, string contentUrl, string title = "", string description = "")
    {
        return new Post(userId, title, description, contentUrl, ContentType.Video);
    }
    
    public void Update(string title, string description)
    {
        ValidateContent(ContentType, title, description, ContentUrl);
        
        Title = title;
        Description = description;
    }
    
    public void IncrementCommentCount() => CommentCount++;
    public void DecrementCommentCount() => CommentCount--;
    
    public void IncrementLikeCount() => LikeCount++;
    public void DecrementLikeCount() => LikeCount--;
    
    private static void ValidateContent(ContentType contentType, string title, string description, string contentUrl)
    {
        if (contentType == ContentType.Text)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty for text content.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty for text content.", nameof(description));
            }
        }

        else if ((contentType == ContentType.Image || contentType == ContentType.Video) && string.IsNullOrWhiteSpace(contentUrl))
        {
            throw new ArgumentException("ContentUrl is required for image or video content.", nameof(contentUrl));
        }
    }
}
