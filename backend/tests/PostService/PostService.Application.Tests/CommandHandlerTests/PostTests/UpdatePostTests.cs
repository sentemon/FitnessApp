using System.Net;
using FluentAssertions;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.DTOs;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.PostTests;

public class UpdatePostTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldUpdatePost_WhenDataIsValid()
    {
        // Arrange
        var userId = Fixture.ExistingUser.Id;
        var post = Fixture.ExistingPost;

        var contentUrl = "https://example.com";
        post.SetContentUrl(contentUrl);
        
        var newTitle = "New Title";
        var newDescription = "New Description";
        
        var updatePost = new UpdatePostDto(post.Id.ToString(), newTitle, newDescription);

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
        var id = Guid.Empty.ToString();
        var newTitle = "New Title";
        var newDescription = "New Description";
        
        var updatePost = new UpdatePostDto(id, newTitle, newDescription);
        var userId = Fixture.ExistingUser.Id;

        var command = new UpdatePostCommand(updatePost, userId);

        // Act
        var result = await Fixture.UpdatePostCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Post not found.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserIdDoesNotMatch()
    {
        // Arrange
        var anotherUserId = Guid.NewGuid().ToString();
        var post = Fixture.ExistingPost;

        var contentUrl = "https://example.com";
        post.SetContentUrl(contentUrl);

        var newTitle = "New Title";
        var newDescription = "New Description";
        
        var updatePost = new UpdatePostDto(post.Id.ToString(), newTitle, newDescription);
        
        var command = new UpdatePostCommand(updatePost, anotherUserId);
        
        // Act
        var result = await Fixture.UpdatePostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("You do not have permission to update this post.");
    }
}