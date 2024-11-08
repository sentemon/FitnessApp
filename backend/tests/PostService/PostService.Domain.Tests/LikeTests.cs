using FluentAssertions;
using PostService.Domain.Entities;
using Xunit;

namespace PostService.Domain.Tests;

public class LikeTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        
        // Act
        var like = new Like(postId, userId);

        // Assert
        like.PostId.Should().Be(postId);
        like.UserId.Should().Be(userId);
        like.CreatedAt.Date.Should().Be(createdAt.Date);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenPostIdIsEmpty()
    {
        // Arrange
        var postId = Guid.Empty;
        var userId = Guid.NewGuid();
        
        // Act
        var act = () => new Like(postId, userId);
        
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
        
        // Act
        var act = () => new Like(postId, userId);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"UserId cannot be empty. (Parameter '{nameof(userId)}')")
            .And.ParamName.Should().Be("userId");
    }
}