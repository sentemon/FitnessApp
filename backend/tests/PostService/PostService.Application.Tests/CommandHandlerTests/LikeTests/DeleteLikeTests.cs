using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.DeleteLike;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.LikeTests;

public class DeleteLikeTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeleteLikeFromPost_WhenDataIsValid()
    {
        // Arrange
        var userId = Fixture.ExistingUser.Id;
        var postId = Fixture.ExistingPost.Id;
        
        var commandLike = new AddLikeCommand(postId, userId);
        
        var like = await Fixture.AddLikeCommandHandler.HandleAsync(commandLike);
        like.Response.Should().NotBeNull();
        
        var command = new DeleteLikeCommand(like.Response.PostId, like.Response.UserId);
        
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
        var postId = Guid.NewGuid();
        var userId = Fixture.ExistingUser.Id;

        var command = new DeleteLikeCommand(postId, userId);

        // Act
        var result = await Fixture.DeleteLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Like not found.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserHasNotLikedPost()
    {
        // Arrange
        var userId = Fixture.ExistingUser.Id;
        var anotherUserId = Guid.NewGuid().ToString();
        
        var postId = Fixture.ExistingPost.Id;
        
        var commandLike = new AddLikeCommand(postId, userId);
        
        var like = await Fixture.AddLikeCommandHandler.HandleAsync(commandLike);
        like.Response.Should().NotBeNull();
        
        var command = new DeleteLikeCommand(like.Response.PostId, anotherUserId);

        // Act
        var result = await Fixture.DeleteLikeCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("User has not liked this post yet.");
    }
}
