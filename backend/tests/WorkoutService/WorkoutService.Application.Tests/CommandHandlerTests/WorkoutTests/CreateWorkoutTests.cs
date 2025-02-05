using FluentAssertions;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.WorkoutTests;

public class CreateWorkoutTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldCreateWorkout_WhenDataIsValid()
    {
        // Arrange
        var title = "Example Title";
        var description = "Example Description";
        var durationInMinutes = 35u;
        var level = DifficultyLevel.Advanced;
        var exerciseDtos = new[]
        {
            new ExerciseDto(
                "Example Name",
                DifficultyLevel.AllLevels,
                [
                    new SetDto(20, 20),
                    new SetDto(30, 10),
                    new SetDto(5, 40)
                ]
            )
        };
        
        var userId = Fixture.ExistingUser.Id;
        var createWorkoutDto = new CreateWorkoutDto(title, description, durationInMinutes, level, null, exerciseDtos);
        
        var command = new CreateWorkoutCommand(createWorkoutDto, userId);
        
        // Act
        var workout = await Fixture.CreateWorkoutCommandHandler.HandleAsync(command);

        // Assert
        workout.IsSuccess.Should().BeTrue();
        workout.Response.Title.Should().Be(title);
        workout.Response.Description.Should().Be(description);
        workout.Response.DurationInMinutes.Should().Be(durationInMinutes);
        workout.Response.Level.Should().Be(level);
        workout.Response.Exercises.Should().Equal(exerciseDtos);
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenValidationFailed()
    {
        // Arrange
        var title = string.Empty;
        var description = new string('*', 501);
        var durationInMinutes = 45u;
        var level = (DifficultyLevel)34;
        var exerciseDtos = Array.Empty<ExerciseDto>();
        
        var userId = Fixture.ExistingUser.Id;
        var createWorkoutDto = new CreateWorkoutDto(title, description, durationInMinutes, level, null, exerciseDtos);
        
        var command = new CreateWorkoutCommand(createWorkoutDto, userId);

        // Act
        var result = await Fixture.CreateWorkoutCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Response.Should().BeNull();
        result.Error.Message.Should().Be("Title of workout cannot be empty.; Description of workout cannot be longer than 500 characters.; There is no that level for workout");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        var title = "Example Title";
        var description = "Example Description";
        var durationInMinutes = 35u;
        var level = DifficultyLevel.Advanced;
        var exerciseDtos = new[]
        {
            new ExerciseDto(
                "Example Name",
                DifficultyLevel.AllLevels,
                [
                    new SetDto(20, 20),
                    new SetDto(30, 10),
                    new SetDto(5, 40)
                ]
            )
        };
        
        var userId = Guid.Empty.ToString();
        var createWorkoutDto = new CreateWorkoutDto(title, description, durationInMinutes, level, null, exerciseDtos);
        
        var command = new CreateWorkoutCommand(createWorkoutDto, userId);
        
        // Act
        var workout = await Fixture.CreateWorkoutCommandHandler.HandleAsync(command);

        // Assert
        workout.IsSuccess.Should().BeFalse();
        workout.Response.Should().BeNull();
        workout.Error.Message.Should().Be(ResponseMessages.UserNotFound);
    }
}