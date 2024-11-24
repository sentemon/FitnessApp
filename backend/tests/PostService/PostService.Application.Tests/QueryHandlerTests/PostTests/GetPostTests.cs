using System.Net;
using FluentAssertions;
using PostService.Application.Commands.AddPost;
using PostService.Application.DTOs;
using PostService.Application.Queries.GetPost;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.QueryHandlerTests.PostTests;

public class GetPostTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldReturnPost_WhenPostIdExists()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var userId = Fixture.ExistingUser.Id;

        var commandAddPost = new AddPostCommand(createPost, userId);

        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost);
        post.Response.Should().NotBeNull();
        
        var query = new GetPostQuery(post.Response.Id);
        
        // Act
        var result = await Fixture.GetPostQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Id.Should().Be(post.Response.Id);
        result.Response.Title.Should().Be(post.Response.Title);
        result.Response.Description.Should().Be(post.Response.Description);
        result.Response.ContentUrl.Should().Be(post.Response.ContentUrl);
        result.Response.ContentType.Should().Be(post.Response.ContentType);
        result.Response.CommentCount.Should().Be(post.Response.CommentCount);
        result.Response.LikeCount.Should().Be(post.Response.LikeCount);
        result.Response.CreatedAt.Should().Be(post.Response.CreatedAt);
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPostIdDoesNotExist()
    {
        // Arrange
        var id = Guid.Empty;

        var query = new GetPostQuery(id);

        // Act
        var result = await Fixture.GetPostQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("Post not found.");
    }
}