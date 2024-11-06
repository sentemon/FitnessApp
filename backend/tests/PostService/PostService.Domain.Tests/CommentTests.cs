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
}