namespace PostService.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public string UserId { get; private set; }
    public string Username { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Comment(Guid postId, string userId, string username, string content)
    {
        if (postId == Guid.Empty)
        {
            throw new ArgumentException("PostId cannot be empty.", nameof(postId));
        }
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }
        
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty or whitespace.", nameof(username));
        }
        
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be empty or whitespace.", nameof(content));
        }
        
        PostId = postId;
        UserId = userId;
        Username = username;
        Content = content;
    }
}