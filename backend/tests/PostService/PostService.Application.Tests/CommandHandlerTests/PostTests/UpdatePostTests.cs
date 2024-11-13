using System.Net;
using FluentAssertions;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.PostTests;

public class UpdatePostTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldUpdatePost_WhenDataIsValid()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var userId = Guid.NewGuid();

        var postCommand = new AddPostCommand(createPost, userId);

        var post = await Fixture.AddPostCommandHandler.HandleAsync(postCommand);
        post.Response.Should().NotBeNull();
        
        var newTitle = "New Title";
        var newDescription = "New Description";
        
        var updatePost = new UpdatePostDto(post.Response.Id, newTitle, newDescription);

        var command = new UpdatePostCommand(updatePost, userId);

        // Act
        var result = await Fixture.UpdatePostCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Title.Should().Be(newTitle);
        result.Response.Description.Should().Be(newDescription);
    }

    [Fact] public async Task HandleAsync_ShouldFail_WhenPostDoesNotExist()
    {
        // Arrange
        var id = Guid.Empty;
        var newTitle = "New Title";
        var newDescription = "New Description";
        
        var updatePost = new UpdatePostDto(id, newTitle, newDescription);
        var userId = Guid.NewGuid();

        var command = new UpdatePostCommand(updatePost, userId);

        // Act
        var result = await Fixture.UpdatePostCommandHandler.HandleAsync(command);

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

        var postCommand = new AddPostCommand(createPost, userId);
        
        var post = await Fixture.AddPostCommandHandler.HandleAsync(postCommand);
        post.Response.Should().NotBeNull();

        var newTitle = "New Title";
        var newDescription = "New Description";
        
        var updatePost = new UpdatePostDto(post.Response.Id, newTitle, newDescription);
        
        var command = new UpdatePostCommand(updatePost, anotherUserId);
        
        // Act
        var result = await Fixture.UpdatePostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("You do not have permission to update this post.");
    }
}