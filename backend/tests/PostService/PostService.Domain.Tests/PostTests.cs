using PostService.Domain.Entities;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Domain.Tests;

public class PostTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Sample Title";
        var description = "Sample Description";
        var contentUrl = "http://example.com/content";
        var contentType = ContentType.Text;

        // Act
        var post = new Post(userId, title, description, contentUrl, contentType);

        // Assert
        Assert.Equal(userId, post.UserId);
        Assert.Equal(title, post.Title);
        Assert.Equal(description, post.Description);
        Assert.Equal(contentUrl, post.ContentUrl);
        Assert.Equal(contentType, post.ContentType);
        Assert.Equal((uint)0, post.LikeCount);
        Assert.Equal((uint)0, post.CommentCount);
        Assert.Equal(DateTime.UtcNow.Date, post.CreatedAt.Date);
    }

    [Fact]
    public void Update_ShouldModifyTitleAndDescription()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(), 
            "Old Title", 
            "Old Description", 
            "http://example.com",
            ContentType.Text);
        
        var newTitle = "New Title";
        var newDescription = "New Description";

        // Act
        post.Update(newTitle, newDescription);

        // Assert
        Assert.Equal(newTitle, post.Title);
        Assert.Equal(newDescription, post.Description);
    }

    [Fact]
    public void IncrementLikeCount_ShouldIncreaseLikeCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(),
            "Title", 
            "Description",
            "http://example.com",
            ContentType.Text);

        // Act
        post.IncrementLikeCount();

        // Assert
        Assert.Equal((uint)1, post.LikeCount);
    }

    [Fact]
    public void DecrementLikeCount_ShouldDecreaseLikeCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(),
            "Title",
            "Description",
            "http://example.com",
            ContentType.Text);
        
        post.IncrementLikeCount(); // Initial increment to avoid negative count

        // Act
        post.DecrementLikeCount();

        // Assert
        Assert.Equal((uint)0, post.LikeCount);
    }

    [Fact]
    public void IncrementCommentCount_ShouldIncreaseCommentCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(), 
            "Title", 
            "Description", 
            "http://example.com", 
            ContentType.Text);

        // Act
        post.IncrementCommentCount();

        // Assert
        Assert.Equal((uint)1, post.CommentCount);
    }

    [Fact]
    public void DecrementCommentCount_ShouldDecreaseCommentCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(),
            "Title", 
            "Description",
            "http://example.com",
            ContentType.Text);
        
        post.IncrementCommentCount(); // Initial increment to avoid negative count

        // Act
        post.DecrementCommentCount();

        // Assert
        Assert.Equal((uint)0, post.CommentCount);
    }
}