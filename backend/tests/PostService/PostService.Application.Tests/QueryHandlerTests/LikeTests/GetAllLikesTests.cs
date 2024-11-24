using System.Net;
using FluentAssertions;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.AddLike;
using PostService.Application.DTOs;
using PostService.Application.Queries.GetAllLikes;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.QueryHandlerTests.LikeTests;

public class GetAllLikesTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldReturnLikes_ForSpecifiedPost()
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

        var postId = post.Response.Id;
        
        var commandLike1 = new AddLikeCommand(postId, Guid.NewGuid());
        var commandLike2 = new AddLikeCommand(postId, Guid.NewGuid());

        var like1 = await Fixture.AddLikeCommandHandler.HandleAsync(commandLike1);
        var like2 = await Fixture.AddLikeCommandHandler.HandleAsync(commandLike2);
        
        var query = new GetAllLikesQuery(postId);

        // Act
        var result = await Fixture.GetAllLikesQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().HaveCount(2);
        result.Response.Should().ContainEquivalentOf(like1.Response);
        result.Response.Should().ContainEquivalentOf(like2.Response);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoLikesExist()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var query = new GetAllLikesQuery(postId);

        // Act
        var result = await Fixture.GetAllLikesQueryHandler.HandleAsync(query);

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
        var userId = Fixture.ExistingUser.Id;

        var commandPost = new AddPostCommand(createPost, userId);
        
        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandPost);
        post.Response.Should().NotBeNull();

        var postId = post.Response.Id;

        var likes = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        foreach (var likeUserId in likes)
        {
            var commandLike = new AddLikeCommand(postId, likeUserId);
            await Fixture.AddLikeCommandHandler.HandleAsync(commandLike);
        }

        var query = new GetAllLikesQuery(postId, First: 2);

        // Act
        var result = await Fixture.GetAllLikesQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().HaveCount(2);
    }
}
