using FluentAssertions;
using WorkoutService.Application.Commands.UpdateWorkout;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.WorkoutTests;

public class UpdateWorkoutTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldUpdateWorkout_WhenDataIsValid()
    {
        // Arrange
        var id = Fixture.ExistingWorkout.Id;
        var title = "New Title";
        var description = "New Description";
        var durationInMinutes = 58u;
        var level = DifficultyLevel.Advanced;
        
        var input = new UpdateWorkoutDto(id, title, description, durationInMinutes, level);
        var command = new UpdateWorkoutCommand(input);

        // Act
        var result = await Fixture.UpdateWorkoutCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        result.Response.Should().Be("Workout is successfully updated");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenValidationFailed()
    {
        // Arrange
        var id = Fixture.ExistingWorkout.Id;
        var title = string.Empty;
        var description = new string('*', 501);
        var durationInMinutes = 45u;
        var level = (DifficultyLevel)34;
        
        var input = new UpdateWorkoutDto(id, title, description, durationInMinutes, level);
        var command = new UpdateWorkoutCommand(input);

        // Act
        var result = await Fixture.UpdateWorkoutCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Title of workout cannot be empty.; Description of workout cannot be longer than 500 characters.; There is no that level for workout");
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenWorkoutNotFound()
    {
        // Arrange
        var id = Guid.Empty;
        var title = "New Title";
        var description = "New Description";
        var durationInMinutes = 58u;
        var level = DifficultyLevel.Advanced;

        var input = new UpdateWorkoutDto(id, title, description, durationInMinutes, level);
        var command = new UpdateWorkoutCommand(input);

        // Act
        var result = await Fixture.UpdateWorkoutCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be("Workout not found.");
    }
}