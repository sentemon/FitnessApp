using FluentAssertions;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.DTOs;
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
}