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
        var userId = Fixture.ExistingUser.Id;

        var commandPost = new AddPostCommand(createPost, userId);
        
        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandPost);
        post.Response.Should().NotBeNull();
        
        var command = new AddLikeCommand(post.Response.Id, userId);
        
        // Act
        var result = await Fixture.AddLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.PostId.Should().Be(post.Response.Id);
        result.Response.UserId.Should().Be(userId);
        
        var likedPost = await Fixture.PostDbContextFixture.Posts
            .FirstAsync(p => p.Id == post.Response.Id);
        likedPost.LikeCount.Should().Be(1);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPostDoesNotExit()
    {
        // Arrange
        var postId = Guid.Empty;
        var userId = Fixture.ExistingUser.Id;
        
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
    public async Task HandleAsync_ShouldFail_WhenUserTriesToLikePostTwice()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var userId = Fixture.ExistingUser.Id;

        var commandPost = new AddPostCommand(createPost, userId);
        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandPost);
        post.Response.Should().NotBeNull();
        
        var command = new AddLikeCommand(post.Response.Id, userId);
        
        var firstResult = await Fixture.AddLikeCommandHandler.HandleAsync(command);
        firstResult.IsSuccess.Should().BeTrue();

        var secondResult = await Fixture.AddLikeCommandHandler.HandleAsync(command);

        // Assert
        secondResult.IsSuccess.Should().BeFalse();
        secondResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        secondResult.Response.Should().BeNull();
        secondResult.Error?.Message.Should().Be("User has already liked this post.");
        
        var likedPost = await Fixture.PostDbContextFixture.Posts
            .FirstAsync(p => p.Id == post.Response.Id);
        likedPost.LikeCount.Should().Be(1);
    }
}