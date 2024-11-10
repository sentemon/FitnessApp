using System.Net;
using FluentAssertions;
using PostService.Application.Commands.AddPost;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.PostTests;

public class AddPostTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldAddPostCorrectly()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var userId = Guid.NewGuid();

        var command = new AddPostCommand(createPost, userId);
        
        // Act
        var result = await Fixture.AddPostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Title.Should().Be(title);
        result.Response.Description.Should().Be(description);
        result.Response.ContentUrl.Should().Be(contentUrl);
        result.Response.ContentType.Should().Be(contentType);
        result.Response.LikeCount.Should().Be(0);
        result.Response.CommentCount.Should().Be(0);
    }
}