using System.Net;
using FluentAssertions;
using PostService.Application.Commands.AddPost;
using PostService.Application.DTOs;
using PostService.Application.Queries.GetAllPosts;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.QueryHandlerTests.PostTests;

public class GetAllPostsTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldReturnAllPosts_WhenNoLastPostIdProvided()
    {
        // Arrange
        var title1 = "Title 1";
        var description1 = "Description 1";
        var contentUrl1 = "https://example.com";
        var contentType = ContentType.Image;

        var createPost1 = new CreatePostDto(title1, description1, contentUrl1, contentType);
        var userId = Guid.NewGuid();

        var commandAddPost1 = new AddPostCommand(createPost1, userId);
        var post1 = await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost1);

        var title2 = "Title 2";
        var description2 = "Description 2";
        var contentUrl2 = "https://example.com";

        var createPost2 = new CreatePostDto(title2, description2, contentUrl2, contentType);
        var commandAddPost2 = new AddPostCommand(createPost2, userId);
        var post2 = await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost2);

        var query = new GetAllPostsQuery(10, Guid.Empty);

        // Act
        var result = await Fixture.GetAllPostsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Should().HaveCount(2);
        result.Response.Should().ContainEquivalentOf(post1.Response);
        result.Response.Should().ContainEquivalentOf(post2.Response);
    }

    [Fact]
    public async Task HandleAsync_ShouldExcludePostWithLastPostId()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentUrl = "https://example.com";
        var contentType = ContentType.Image;
        var userId = Guid.NewGuid();

        var createPost = new CreatePostDto(title, description, contentUrl, contentType);
        var commandAddPost = new AddPostCommand(createPost, userId);
        
        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost);
        post.Response.Should().NotBeNull();

        var anotherPost = new CreatePostDto("Another Title", "Another Description", "https://example.com", contentType);
        var anotherCommand = new AddPostCommand(anotherPost, userId);
        var anotherPostResult = await Fixture.AddPostCommandHandler.HandleAsync(anotherCommand);

        var query = new GetAllPostsQuery(10, post.Response.Id);

        // Act
        var result = await Fixture.GetAllPostsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Should().ContainEquivalentOf(anotherPostResult.Response);
    }

    [Fact]
    public async Task HandleAsync_ShouldLimitResults_WhenFirstIsSpecified()
    {
        // Arrange
        var title1 = "Title 1";
        var description1 = "Description 1";
        var contentUrl1 = "https://example.com";
        var contentType = ContentType.Image;
        var userId = Guid.NewGuid();

        var createPost1 = new CreatePostDto(title1, description1, contentUrl1, contentType);
        var commandAddPost1 = new AddPostCommand(createPost1, userId);
        await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost1);

        var title2 = "Title 2";
        var description2 = "Description 2";
        var contentUrl2 = "https://example.com";

        var createPost2 = new CreatePostDto(title2, description2, contentUrl2, contentType);
        var commandAddPost2 = new AddPostCommand(createPost2, userId);
        await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost2);

        var query = new GetAllPostsQuery(1, Guid.Empty);

        // Act
        var result = await Fixture.GetAllPostsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Should().HaveCount(1);
    }
}
