using FluentAssertions;
using WorkoutService.Application.Commands.MarkSetHistoryAsCompleted;
using WorkoutService.Domain.Constants;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.SetHistoryTests;

public class MarkSetHistoryAsCompletedTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldMarkSetHistoryAsCompleted_WhenDataIsValid()
    {
        // Arrange
        var setHistoryId = Fixture.ExistingSetHistory.Id;
        var command = new MarkSetHistoryAsCompletedCommand(setHistoryId);

        // Act
        var result = await Fixture.MarkSetHistoryAsCompletedCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be(ResponseMessages.SetCompleted);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenExerciseHistoryOrSetHistoryNotFound()
    {
        // Arrange
        var setHistoryId = Guid.Empty;
        var command = new MarkSetHistoryAsCompletedCommand(setHistoryId);

        // Act
        var result = await Fixture.MarkSetHistoryAsCompletedCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.SetHistoryNotFound);
    }
}