using System.Diagnostics;
using System.Net;
using FluentAssertions;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeletePost;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.PostTests;

public class DeletePostTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeletePost_WhenPostExists()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var userId = Fixture.ExistingUser.Id;

        var postCommand = new AddPostCommand(createPost, userId);
        
        // Act
        var post = await Fixture.AddPostCommandHandler.HandleAsync(postCommand);

        Debug.Assert(post.Response != null, "post.Response != null");
        var command = new DeletePostCommand(post.Response.Id, userId);
        
        // Act
        var result = await Fixture.DeletePostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().Be("You successfully deleted post.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPostDoesNotExist()
    {
        // Arrange
        var id = Guid.Empty;
        var userId = Fixture.ExistingUser.Id;
        
        // Act
        var command = new DeletePostCommand(id, userId);
        
        // Act
        var result = await Fixture.DeletePostCommandHandler.HandleAsync(command);
        
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
        var userId = Fixture.ExistingUser.Id;
        var anotherUserId = Guid.NewGuid();

        var postCommand = new AddPostCommand(createPost, userId);
        
        // Act
        var post = await Fixture.AddPostCommandHandler.HandleAsync(postCommand);
        post.Response.Should().NotBeNull();
        
        var command = new DeletePostCommand(post.Response.Id, anotherUserId);
        
        // Act
        var result = await Fixture.DeletePostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("You do not have permission to delete this post.");
    }
}