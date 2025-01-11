using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Commands.AddComment;
using PostService.Application.Commands.AddPost;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.CommentTests;

public class AddCommentTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldCommentPost_WhenDataIsValid()
    {
        // Arrange
        var userId = Fixture.ExistingUser.Id;
        var postId = Fixture.ExistingPost.Id;
        var postCommentCount = Fixture.ExistingPost.CommentCount;

        var content = "This is a comment";
        
        var createComment = new CreateCommentDto(postId, content);

        var command = new AddCommentCommand(createComment, userId);

        // Act
        var result = await Fixture.AddCommentCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.PostId.Should().Be(postId);
        result.Response.UserId.Should().Be(userId);

        var commentedPost = await Fixture.PostDbContextFixture.Posts
            .FirstAsync(p => p.Id == postId);
        commentedPost.CommentCount.Should().Be(postCommentCount + 1);
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPostDoesNotExist()
    {
        // Arrange
        var userId = Fixture.ExistingUser.Id;
        var postId = Guid.Empty;
        var content = "This is a comment";
        var createComment = new CreateCommentDto(postId, content);

        var command = new AddCommentCommand(createComment, userId);

        // Act
        var result = await Fixture.AddCommentCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("Post not found.");
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenCommentTooLong()
    {
        var userId = Fixture.ExistingUser.Id;
        var postId = Fixture.ExistingPost.Id;

        var content = new string('*', 513);
        
        var createComment = new CreateCommentDto(postId, content);

        var command = new AddCommentCommand(createComment, userId);

        // Act
        var result = await Fixture.AddCommentCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Comment cannot be longer than 512 characters");
    }

    [Fact]
    public async Task HandleAsync_ShouldAllowMultipleCommentsFromUser()
    {
        // Arrange
        var userId = Fixture.ExistingUser.Id;
        var postId = Fixture.ExistingPost.Id;

        var content = "This is the first comment";
        
        var createComment = new CreateCommentDto(postId, content);

        var command = new AddCommentCommand(createComment, userId);

        var firstResult = await Fixture.AddCommentCommandHandler.HandleAsync(command);
        firstResult.IsSuccess.Should().BeTrue();

        var secondContent = "This is the second comment";
        var secondCreateComment = new CreateCommentDto(postId, secondContent);
        var secondCommand = new AddCommentCommand(secondCreateComment, userId);

        var secondResult = await Fixture.AddCommentCommandHandler.HandleAsync(secondCommand);

        // Assert
        secondResult.IsSuccess.Should().BeTrue();
        secondResult.StatusCode.Should().Be(HttpStatusCode.OK);
        secondResult.Response.Should().NotBeNull();
        secondResult.Response.Content.Should().Be(secondContent);

        var commentedPost = await Fixture.PostDbContextFixture.Posts
            .FirstAsync(p => p.Id == postId);
        commentedPost.CommentCount.Should().Be(2);
    }
}