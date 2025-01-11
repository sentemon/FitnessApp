using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using PostService.Application.Commands.AddPost;
using PostService.Application.DTOs;
using PostService.Domain.Enums;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.PostTests;

public class AddPostTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldAddPost_WhenDataIsValid()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        await using var stream = new MemoryStream([1, 2, 3]);
        var fileName = "file.jpg";
        var contentTypeFile = "image/jpeg";

        var file = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentTypeFile
        };
        
        var contentType = ContentType.Image;

        var createPost = new CreatePostDto(title, description, file, contentTypeFile, contentType);
        var userId = Fixture.ExistingUser.Id;

        var command = new AddPostCommand(createPost, userId);
        
        // Act
        var result = await Fixture.AddPostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Title.Should().Be(title);
        result.Response.Description.Should().Be(description);
        result.Response.ContentType.Should().Be(ContentType.Image);
        result.Response.LikeCount.Should().Be(0);
        result.Response.CommentCount.Should().Be(0);
    }


    [Fact]
    public async Task HandleAsync_ShouldFail_WhenTitleIsEmpty_ForTextContentType()
    {
        var title = "";
        var description = "Description";
        await using var stream = new MemoryStream([1, 2, 3]);
        var fileName = "aaa.txt";
        var contentTypeFile = "text/plain";

        var file = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentTypeFile
        };
        
        var contentType = ContentType.Text;
        
        var createPost = new CreatePostDto(title, description, file, contentTypeFile, contentType);
        var userId = Fixture.ExistingUser.Id;

        var command = new AddPostCommand(createPost, userId);
        
        // Act
        var result = await Fixture.AddPostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Title is required for text content.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenDescriptionIsEmpty_ForTextContentType()
    {
        var title = "Title";
        var description = "";
        var contentType = ContentType.Text;

        var createPost = new CreatePostDto(title, description, null, null, contentType);
        var userId = Fixture.ExistingUser.Id;

        var command = new AddPostCommand(createPost, userId);
        
        // Act
        var result = await Fixture.AddPostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Description is required for text content.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenTitleAndDescriptionAreEmpty_ForTextContentType()
    {
        var title = "";
        var description = "";
        var contentType = ContentType.Text;

        var createPost = new CreatePostDto(title, description, null, null, contentType);
        var userId = Fixture.ExistingUser.Id;

        var command = new AddPostCommand(createPost, userId);
        
        // Act
        var result = await Fixture.AddPostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Title is required for text content.; Description is required for text content.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenContentTypeIsInvalid()
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var contentType = (ContentType)23;

        await using var stream = new MemoryStream();
        var fileName = "invalid_file.txt";
        var file = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        var createPost = new CreatePostDto(title, description, file, file.ContentType, contentType);
        var userId = Fixture.ExistingUser.Id;

        var command = new AddPostCommand(createPost, userId);
        
        // Act
        var result = await Fixture.AddPostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("ContentType must be one of the following: Text, Image, Video.");
    }
}