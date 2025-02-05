using FluentAssertions;
using WorkoutService.Application.Commands.AddSet;
using WorkoutService.Domain.Constants;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.SetTests;

public class AddSetTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldAddSet_WhenDataIsValid()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseId = Fixture.ExistingExercise.Id;

        var command = new AddSetCommand(reps, weight, exerciseId);
        
        // Act
        var result = await Fixture.AddSetCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Reps.Should().Be(reps);
        result.Response.Weight.Should().Be(weight);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenExerciseNotFound()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var notExistingExerciseId = Guid.Empty;

        var command = new AddSetCommand(reps, weight, notExistingExerciseId);
        
        // Act
        var result = await Fixture.AddSetCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.ExerciseNotFound);
    }
}