using PostService.Domain.Entities;
using PostService.Domain.Enums;
using Xunit;
using FluentAssertions;

namespace PostService.Domain.Tests;

public class PostTests
{
    [Fact]
    public void CreateTextPost_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Sample Title";
        var description = "Sample Description";

        // Act
        var post = Post.CreateTextPost(userId, title, description);

        // Assert
        post.UserId.Should().Be(userId);
        post.Title.Should().Be(title);
        post.Description.Should().Be(description);
        post.ContentUrl.Should().Be(string.Empty);
        post.ContentType.Should().Be(ContentType.Text);
        post.LikeCount.Should().Be(0);
        post.CommentCount.Should().Be(0);
        post.CreatedAt.Date.Should().Be(DateTime.UtcNow.Date);
    }

    [Fact]
    public void CreateImagePost_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Sample Title";
        var description = "Sample Description";
        var contentUrl = "https://example.com/image.jpg";

        // Act
        var post = Post.CreateImagePost(userId, contentUrl, title, description);

        // Assert
        post.UserId.Should().Be(userId);
        post.Title.Should().Be(title);
        post.Description.Should().Be(description);
        post.ContentUrl.Should().Be(contentUrl);
        post.ContentType.Should().Be(ContentType.Image);
        post.LikeCount.Should().Be(0);
        post.CommentCount.Should().Be(0);
        post.CreatedAt.Date.Should().Be(DateTime.UtcNow.Date);
    }

    [Fact]
    public void CreateVideoPost_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Sample Title";
        var description = "Sample Description";
        var contentUrl = "https://example.com/video.mp4";

        // Act
        var post = Post.CreateVideoPost(userId, contentUrl, title, description);

        // Assert
        post.UserId.Should().Be(userId);
        post.Title.Should().Be(title);
        post.Description.Should().Be(description);
        post.ContentUrl.Should().Be(contentUrl);
        post.ContentType.Should().Be(ContentType.Video);
        post.LikeCount.Should().Be(0);
        post.CommentCount.Should().Be(0);
        post.CreatedAt.Date.Should().Be(DateTime.UtcNow.Date);
    }

    [Fact]
    public void Update_ShouldModifyTitleAndDescription()
    {
        // Arrange
        var post = Post.CreateTextPost(
            Guid.NewGuid(), 
            "Old Title", 
            "Old Description");

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
        var post = Post.CreateTextPost(
            Guid.NewGuid(),
            "Title", 
            "Description");

        // Act
        post.IncrementLikeCount();

        // Assert
        post.LikeCount.Should().Be(1);
    }

    [Fact]
    public void DecrementLikeCount_ShouldDecreaseLikeCountByOne()
    {
        // Arrange
        var post = Post.CreateTextPost(
            Guid.NewGuid(),
            "Title",
            "Description");
        
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
        var post = Post.CreateTextPost(
            Guid.NewGuid(), 
            "Title", 
            "Description");

        // Act
        post.IncrementCommentCount();

        // Assert
        post.CommentCount.Should().Be(1);
    }

    [Fact]
    public void DecrementCommentCount_ShouldDecreaseCommentCountByOne()
    {
        // Arrange
        var post = Post.CreateTextPost(
            Guid.NewGuid(),
            "Title", 
            "Description");
        
        post.IncrementCommentCount(); // Initial increment to avoid negative count

        // Act
        post.DecrementCommentCount();

        // Assert
        post.CommentCount.Should().Be(0);
    }

    [Fact]
    public void CreateTextPost_ShouldThrowArgumentException_WhenUserIdIsEmpty()
    {
        // Arrange
        var userId = Guid.Empty;
        var title = "Title";
        var description = "Description";
        
        // Act
        var act = () => Post.CreateTextPost(userId, title, description);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"UserId cannot be empty. (Parameter '{nameof(userId)}')")
            .And.ParamName.Should().Be("userId");
    }

    [Fact]
    public void CreateTextPost_ShouldThrowArgumentException_WhenTitleIsEmpty_ForTextContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "";
        var description = "Description";
        
        // Act
        var act = () => Post.CreateTextPost(userId, title, description);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"Title cannot be empty for text content. (Parameter '{nameof(title)}')")
            .And.ParamName.Should().Be("title");
    }

    [Fact]
    public void CreateTextPost_ShouldThrowArgumentException_WhenDescriptionIsEmpty_ForTextContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Title";
        var description = "";
        
        // Act
        var act = () => Post.CreateTextPost(userId, title, description);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"Description cannot be empty for text content. (Parameter '{nameof(description)}')")
            .And.ParamName.Should().Be("description");
    }

    [Fact]
    public void CreateImagePost_ShouldThrowArgumentException_WhenContentUrlIsEmpty_ForImageContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Title";
        var description = "Description";
        var contentUrl = "";
        
        // Act
        var act = () => Post.CreateImagePost(userId, contentUrl, title, description);

        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"ContentUrl is required for image or video content. (Parameter '{nameof(contentUrl)}')")
            .And.ParamName.Should().Be("contentUrl");
    }

    [Fact]
    public void CreateVideoPost_ShouldThrowArgumentException_WhenContentUrlIsEmpty_ForVideoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Title";
        var description = "Description";
        var contentUrl = "";
        
        // Act
        var act = () => Post.CreateVideoPost(userId, contentUrl, title, description);

        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"ContentUrl is required for image or video content. (Parameter '{nameof(contentUrl)}')")
            .And.ParamName.Should().Be("contentUrl");
    }
}