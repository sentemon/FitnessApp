using FluentAssertions;
using WorkoutService.Application.Commands.AddSet;
using WorkoutService.Application.Commands.DeleteSet;
using WorkoutService.Domain.Constants;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.SetTests;

public class DeleteSetTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeleteSet_WhenDataIsValid()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseId = Fixture.ExistingExercise.Id;

        var addSetCommand = new AddSetCommand(reps, weight, exerciseId);
        var set = await Fixture.AddSetCommandHandler.HandleAsync(addSetCommand);

        var command = new DeleteSetCommand(set.Response.Id, exerciseId);
        
        // Act
        var result = await Fixture.DeleteSetCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be(ResponseMessages.SetDeleted);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenExerciseNotFound()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseId = Fixture.ExistingExercise.Id;

        var addSetCommand = new AddSetCommand(reps, weight, exerciseId);
        var set = await Fixture.AddSetCommandHandler.HandleAsync(addSetCommand);
        var notExistingExerciseId = Guid.Empty;

        var command = new DeleteSetCommand(set.Response.Id, notExistingExerciseId);
        
        // Act
        var result = await Fixture.DeleteSetCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.ExerciseNotFound);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenSetNotFound()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseId = Fixture.ExistingExercise.Id;

        var addSetCommand = new AddSetCommand(reps, weight, exerciseId);
        await Fixture.AddSetCommandHandler.HandleAsync(addSetCommand);
        var notExistingSetId = Guid.Empty;

        var command = new DeleteSetCommand(notExistingSetId, exerciseId);
        
        // Act
        var result = await Fixture.DeleteSetCommandHandler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.SetNotFound);
    }
}