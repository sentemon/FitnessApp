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
        await Fixture.MarkSetHistoryAsCompletedCommandHandler.HandleAsync(new MarkSetHistoryAsCompletedCommand(setHistoryId));
        var command = new MarkSetHistoryAsUncompletedCommand(setHistoryId);
        
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
        var setHistoryId = Guid.Empty;
        await Fixture.MarkSetHistoryAsCompletedCommandHandler.HandleAsync(new MarkSetHistoryAsCompletedCommand(setHistoryId));
        var command = new MarkSetHistoryAsUncompletedCommand(setHistoryId);
        
        // Act
        var result = await Fixture.MarkSetHistoryAsUncompletedCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.SetHistoryNotFound);
    }
}