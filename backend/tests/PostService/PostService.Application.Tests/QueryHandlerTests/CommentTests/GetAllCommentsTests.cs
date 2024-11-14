using System.Net;
using FluentAssertions;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.AddComment;
using PostService.Application.DTOs;
using PostService.Application.Queries.GetAllComments;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.QueryHandlerTests.CommentTests;

public class GetAllCommentsTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldReturnComments_ForSpecifiedPost()
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

        var postId = post.Response.Id;

        var commandComment1 = new AddCommentCommand(new CreateCommentDto(postId, "First comment"), userId);
        var commandComment2 = new AddCommentCommand(new CreateCommentDto(postId, "Second comment"), userId);

        var comment1 = await Fixture.AddCommentCommandHandler.HandleAsync(commandComment1);
        var comment2 = await Fixture.AddCommentCommandHandler.HandleAsync(commandComment2);
        
        var query = new GetAllCommentsQuery(postId);

        // Act
        var result = await Fixture.GetAllCommentsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().HaveCount(2);
        result.Response.Should().ContainEquivalentOf(comment1.Response);
        result.Response.Should().ContainEquivalentOf(comment2.Response);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoCommentsExist()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var query = new GetAllCommentsQuery(postId);

        // Act
        var result = await Fixture.GetAllCommentsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_ShouldLimitResults_ToSpecifiedFirstParameter()
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

        var postId = post.Response.Id;


        var comments = new List<CreateCommentDto>
        {
            new(postId, "First comment"),
            new(postId, "Second comment"),
            new(postId, "Third comment")
        };

        foreach (var commentDto in comments)
        {
            var commandComment = new AddCommentCommand(commentDto, userId);
            await Fixture.AddCommentCommandHandler.HandleAsync(commandComment);
        }

        var query = new GetAllCommentsQuery(postId, First: 2);

        // Act
        var result = await Fixture.GetAllCommentsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().HaveCount(2);
    }
}
