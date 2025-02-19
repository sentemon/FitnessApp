using FluentAssertions;
using WorkoutService.Application.Commands.MarkSetHistoryAsCompleted;
using WorkoutService.Application.Commands.MarkSetHistoryAsUncompleted;
using WorkoutService.Domain.Constants;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.SetHistoryTests;

public class MarkSetHistoryAsUncompletedTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldMarkSetHistoryAsUncompleted_WhenDataIsValid()
    {
        // Arrange
        var setHistoryId = Fixture.ExistingSetHistory.Id;
        var exerciseHistoryId = Fixture.ExistingExerciseHistory.Id;
        await Fixture.MarkSetHistoryAsCompletedCommandHandler.HandleAsync(new MarkSetHistoryAsCompletedCommand(setHistoryId, exerciseHistoryId));
        var command = new MarkSetHistoryAsUncompletedCommand(setHistoryId, exerciseHistoryId);
        
        // Act
        var result = await Fixture.MarkSetHistoryAsUncompletedCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be(ResponseMessages.SetUncompleted);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenExerciseHistoryOrSetHistoryNotFound()
    {
        // Arrange
        var exerciseHistoryId = Guid.Empty;
        var setHistoryId = Guid.Empty;
        await Fixture.MarkSetHistoryAsCompletedCommandHandler.HandleAsync(new MarkSetHistoryAsCompletedCommand(setHistoryId, exerciseHistoryId));
        var command = new MarkSetHistoryAsUncompletedCommand(setHistoryId, exerciseHistoryId);
        
        // Act
        var result = await Fixture.MarkSetHistoryAsUncompletedCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.SetHistoryNotFound);
    }
}