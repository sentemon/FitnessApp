using System.Net;
using FluentAssertions;
using PostService.Application.Commands.DeletePost;
using Xunit;

namespace PostService.Application.Tests.CommandHandlerTests.PostTests;

public class DeletePostTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeletePost_WhenPostExists()
    {
        // ToDo: check at home
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand(id, userId);
        
        // Act
        var result = await Fixture.DeletePostCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Response.Should().NotBeNull();
        result.Response.Should().Be("You successfully deleted post.");
    }
}