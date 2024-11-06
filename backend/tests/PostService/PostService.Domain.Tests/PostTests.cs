using PostService.Domain.Entities;
using PostService.Domain.Enums;
using Xunit;
using FluentAssertions;

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
        var contentUrl = "https://example.com/content";
        var contentType = ContentType.Text;

        // Act
        var post = new Post(userId, title, description, contentUrl, contentType);

        // Assert
        post.UserId.Should().Be(userId);
        post.Title.Should().Be(title);
        post.Description.Should().Be(description);
        post.ContentUrl.Should().Be(contentUrl);
        post.ContentType.Should().Be(contentType);
        post.LikeCount.Should().Be(0);
        post.CommentCount.Should().Be(0);
        post.CreatedAt.Date.Should().Be(DateTime.UtcNow.Date);
    }

    [Fact]
    public void Update_ShouldModifyTitleAndDescription()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(), 
            "Old Title", 
            "Old Description", 
            "https://example.com",
            ContentType.Text);
        
        var newTitle = "New Title";
        var newDescription = "New Description";

        // Act
        post.Update(newTitle, newDescription);

        // Assert
        post.Title.Should().Be(newTitle);
        post.Description.Should().Be(newDescription);
    }

    [Fact]
    public void IncrementLikeCount_ShouldIncreaseLikeCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(),
            "Title", 
            "Description",
            "https://example.com",
            ContentType.Text);

        // Act
        post.IncrementLikeCount();

        // Assert
        post.LikeCount.Should().Be(1);
    }

    [Fact]
    public void DecrementLikeCount_ShouldDecreaseLikeCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(),
            "Title",
            "Description",
            "https://example.com",
            ContentType.Text);
        
        post.IncrementLikeCount(); // Initial increment to avoid negative count

        // Act
        post.DecrementLikeCount();

        // Assert
        post.LikeCount.Should().Be(0);
    }

    [Fact]
    public void IncrementCommentCount_ShouldIncreaseCommentCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(), 
            "Title", 
            "Description", 
            "https://example.com", 
            ContentType.Text);

        // Act
        post.IncrementCommentCount();

        // Assert
        post.CommentCount.Should().Be(1);
    }

    [Fact]
    public void DecrementCommentCount_ShouldDecreaseCommentCountByOne()
    {
        // Arrange
        var post = new Post(
            Guid.NewGuid(),
            "Title", 
            "Description",
            "https://example.com",
            ContentType.Text);
        
        post.IncrementCommentCount(); // Initial increment to avoid negative count

        // Act
        post.DecrementCommentCount();

        // Assert
        post.CommentCount.Should().Be(0);
    }
}