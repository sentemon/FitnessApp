using System.Diagnostics;
using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.AddPost;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.LikeTests;

public class AddLikeTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldLikePost_WhenDateIsValid()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var userId = Guid.NewGuid();

        var commandPost = new AddPostCommand(createPost, userId);
        
        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandPost);

        Debug.Assert(post.Response != null, "post.Response != null");
        var command = new AddLikeCommand(post.Response.Id, userId);
        
        // Act
        var result = await Fixture.AddLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.PostId.Should().Be(post.Response.Id);
        result.Response.UserId.Should().Be(userId);
        Fixture.PostDbContextFixture.Posts.First(p => p.Id == post.Response.Id)
            .LikeCount.Should().Be(1);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPostDoesNotExit()
    {
        // Arrange
        var postId = Guid.Empty;
        var userId = Guid.NewGuid();
        
        var command = new AddLikeCommand(postId, userId);
        
        // Act
        var result = await Fixture.AddLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("Post not found.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserIdDoesNotMatch()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();

        var commandPost = new AddPostCommand(createPost, userId);
        
        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandPost);

        Debug.Assert(post.Response != null, "post.Response != null");
        var command = new AddLikeCommand(post.Response.Id, anotherUserId);
        
        // Act
        var result = await Fixture.AddLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("You do not have permission to like this post.");
    }
}