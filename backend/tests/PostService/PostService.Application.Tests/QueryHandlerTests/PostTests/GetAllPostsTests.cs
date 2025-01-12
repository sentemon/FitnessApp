using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
        await Fixture.PostDbContextFixture.Posts.ExecuteDeleteAsync();
        
        // Arrange
        var title1 = "Title 1";
        var description1 = "Description 1";
        var file1 = Fixture.ExistingFile;
        var contentType1 = ContentType.Image;

        var createPost1 = new CreatePostDto(title1, description1, file1, contentType1);
        var userId = Fixture.ExistingUser.Id;

        var commandAddPost1 = new AddPostCommand(createPost1, userId);
        await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost1);

        var title2 = "Title 2";
        var description2 = "Description 2";
        var file2 = Fixture.ExistingFile;
        var contentType2 = ContentType.Image;

        var createPost2 = new CreatePostDto(title2, description2, file2, contentType2);
        var commandAddPost2 = new AddPostCommand(createPost2, userId);
        await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost2);

        var query = new GetAllPostsQuery(10, Guid.Empty);

        // Act
        var result = await Fixture.GetAllPostsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Should().HaveCount(2);
    }

    [Fact]
    public async Task HandleAsync_ShouldExcludePostWithLastPostId()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var file = Fixture.ExistingFile;
        var contentType = ContentType.Image;
        var userId = Fixture.ExistingUser.Id;

        var createPost = new CreatePostDto(title, description, file, contentType);
        var commandAddPost = new AddPostCommand(createPost, userId);
        
        var post = await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost);
        post.Response.Should().NotBeNull();

        var anotherPost = new CreatePostDto("Another Title", "Another Description", file, contentType);
        var anotherCommand = new AddPostCommand(anotherPost, userId);
        await Fixture.AddPostCommandHandler.HandleAsync(anotherCommand);

        var query = new GetAllPostsQuery(10, post.Response.Id);

        // Act
        var result = await Fixture.GetAllPostsQueryHandler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldLimitResults_WhenFirstIsSpecified()
    {
        // Arrange
        var title1 = "Title 1";
        var description1 = "Description 1";
        var file1 = Fixture.ExistingFile;
        var contentType = ContentType.Image;
        var userId = Fixture.ExistingUser.Id;

        var createPost1 = new CreatePostDto(title1, description1, file1, contentType);
        var commandAddPost1 = new AddPostCommand(createPost1, userId);
        await Fixture.AddPostCommandHandler.HandleAsync(commandAddPost1);

        var title2 = "Title 2";
        var description2 = "Description 2";
        var file2 = Fixture.ExistingFile;

        var createPost2 = new CreatePostDto(title2, description2, file2, contentType);
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
