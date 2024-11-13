using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeleteLike;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.LikeTests;

public class DeleteLikeTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeleteLikeFromPost_WhenDataIsValid()
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
        post.Response.Should().NotBeNull();
        
        var commandLike = new AddLikeCommand(post.Response.Id, userId);
        
        var like = await Fixture.AddLikeCommandHandler.HandleAsync(commandLike);
        like.Response.Should().NotBeNull();
        
        var command = new DeleteLikeCommand(like.Response.Id, like.Response.PostId, like.Response.UserId);
        
        // Act 
        var result = await Fixture.DeleteLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        
        var isLikeStillExists = await Fixture.PostDbContextFixture.Likes
            .AnyAsync(l => l.Id == like.Response.Id);

        isLikeStillExists.Should().BeFalse("the like should be deleted from the database after the command execution.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenLikeNotFound()
    {
        // Arrange
        var nonExistentLikeId = Guid.Empty;
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeleteLikeCommand(nonExistentLikeId, postId, userId);

        // Act
        var result = await Fixture.DeleteLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("Like not found.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserHasNotLikedPost()
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
        post.Response.Should().NotBeNull();
        
        var commandLike = new AddLikeCommand(post.Response.Id, userId);
        
        var like = await Fixture.AddLikeCommandHandler.HandleAsync(commandLike);
        like.Response.Should().NotBeNull();
        
        var command = new DeleteLikeCommand(like.Response.Id, like.Response.PostId, anotherUserId);

        // Act
        var result = await Fixture.DeleteLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("User has not liked this post yet.");
    }
}
