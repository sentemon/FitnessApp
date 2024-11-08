using FluentAssertions;
using PostService.Domain.Entities;
using Xunit;

namespace PostService.Domain.Tests;

public class CommentTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var content = "Example Content";
        var createdAt = DateTime.UtcNow;

        // Act
        var comment = new Comment(postId, userId, content);

        // Assert
        comment.PostId.Should().Be(postId);
        comment.UserId.Should().Be(userId);
        comment.Content.Should().Be(content);
        comment.CreatedAt.Date.Should().Be(createdAt.Date);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenPostIdIsEmpty()
    {
        // Arrange
        var postId = Guid.Empty;
        var userId = Guid.NewGuid();
        var content = "Content";
        
        // Act
        var act = () => new Comment(postId, userId, content);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"PostId cannot be empty. (Parameter '{nameof(postId)}')")
            .And.ParamName.Should().Be("postId");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenUserIdIsEmpty()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.Empty;
        var content = "Content";
        
        // Act
        var act = () => new Comment(postId, userId, content);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"UserId cannot be empty. (Parameter '{nameof(userId)}')")
            .And.ParamName.Should().Be("userId");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenContentIsEmpty()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var content = "";
        
        // Act
        var act = () => new Comment(postId, userId, content);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"Content cannot be empty or whitespace. (Parameter '{nameof(content)}')")
            .And.ParamName.Should().Be("content");
    }
}