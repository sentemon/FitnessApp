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
}