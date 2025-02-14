using FluentAssertions;
using WorkoutService.Application.Commands.AddSetHistory;
using WorkoutService.Application.Commands.DeleteSetHistory;
using WorkoutService.Domain.Constants;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.SetHistoryTests;

public class DeleteSetHistoryTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeleteSetHistory_WhenDataIsValid()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseHistoryId = Fixture.ExistingExerciseHistory.Id;

        var addSetHistoryCommand = new AddSetHistoryCommand(reps, weight, exerciseHistoryId);
        var setHistory = await Fixture.AddSetHistoryCommandHandler.HandleAsync(addSetHistoryCommand);

        var command = new DeleteSetHistoryCommand(setHistory.Response.Id, exerciseHistoryId);
        
        // Act
        var result = await Fixture.DeleteSetHistoryCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be(ResponseMessages.SetHistoryDeleted);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenExerciseNotFound()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseHistoryId = Fixture.ExistingExerciseHistory.Id;

        var addSetHistoryCommand = new AddSetHistoryCommand(reps, weight, exerciseHistoryId);
        var setHistory = await Fixture.AddSetHistoryCommandHandler.HandleAsync(addSetHistoryCommand);
        var notExistingExerciseHistoryId = Guid.Empty;

        var command = new DeleteSetHistoryCommand(setHistory.Response.Id, notExistingExerciseHistoryId);
        
        // Act
        var result = await Fixture.DeleteSetHistoryCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.ExerciseHistoryNotFound);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenSetNotFound()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseHistoryId = Fixture.ExistingExerciseHistory.Id;

        var addSetHistoryCommand = new AddSetHistoryCommand(reps, weight, exerciseHistoryId);
        await Fixture.AddSetHistoryCommandHandler.HandleAsync(addSetHistoryCommand);
        var notExistingSetHistoryId = Guid.Empty;

        var command = new DeleteSetHistoryCommand(notExistingSetHistoryId, exerciseHistoryId);
        
        // Act
        var result = await Fixture.DeleteSetHistoryCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.SetHistoryNotFound);
    }
}