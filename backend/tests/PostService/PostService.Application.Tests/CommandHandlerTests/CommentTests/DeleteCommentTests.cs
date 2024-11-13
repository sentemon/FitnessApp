using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PostService.Application.Commands.DeleteComment;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.AddComment;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.CommentTests;

public class DeleteCommentTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeleteComment_WhenUserHasPermission()
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

        var content = "This is a comment";
        
        var createComment = new CreateCommentDto(post.Response.Id, content);
        
        var commandComment = new AddCommentCommand(createComment, userId);
        var comment = await Fixture.AddCommentCommandHandler.HandleAsync(commandComment);
        comment.Response.Should().NotBeNull();
        
        var command = new DeleteCommentCommand(comment.Response.Id, post.Response.Id, userId);

        // Act
        var result = await Fixture.DeleteCommentCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().Be("You successfully deleted comment.");

        var deletedComment = await Fixture.PostDbContextFixture.Comments
            .FirstOrDefaultAsync(c => c.Id == comment.Response.Id);
        deletedComment.Should().BeNull();

        var updatedPost = await Fixture.PostDbContextFixture.Posts
            .FirstAsync(p => p.Id == post.Response.Id);
        updatedPost.CommentCount.Should().Be(0);
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenCommentNotFound()
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

        var id = Guid.Empty;
        
        var deleteCommand = new DeleteCommentCommand(id, post.Response.Id, userId);

        // Act
        var result = await Fixture.DeleteCommentCommandHandler.HandleAsync(deleteCommand);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("Comment not found.");
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserDoesNotHavePermissionToDeleteComment()
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

        var content = "This is a comment";
        
        var createComment = new CreateCommentDto(post.Response.Id, content);
        
        var commandComment = new AddCommentCommand(createComment, userId);
        var comment = await Fixture.AddCommentCommandHandler.HandleAsync(commandComment);
        comment.Response.Should().NotBeNull();

        var command = new DeleteCommentCommand(comment.Response.Id, post.Response.Id, anotherUserId);

        // Act
        var result = await Fixture.DeleteCommentCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("You do not have permission to delete this comment.");
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPostNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var postId = Guid.Empty;
        var userId = Guid.NewGuid();

        var command = new DeleteCommentCommand(id, postId, userId);

        // Act
        var result = await Fixture.DeleteCommentCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error?.Message.Should().Be("Post not found.");
    }
}
