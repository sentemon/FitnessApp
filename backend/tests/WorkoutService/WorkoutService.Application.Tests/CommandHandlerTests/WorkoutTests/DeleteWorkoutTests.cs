using FluentAssertions;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.WorkoutTests;

public class DeleteWorkoutTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldDeleteWorkout_WhenDataIsValid()
    {
        // Arrange
        var title = "Example Title";
        var description = "Example Description";
        var durationInMinutes = 35;
        var level = DifficultyLevel.Advanced;
        var exerciseDtos = new[]
        {
            new CreateExerciseDto(
                "Example Name",
                DifficultyLevel.AllLevels,
                [
                    new CreateSetDto(20, 20),
                    new CreateSetDto(30, 10),
                    new CreateSetDto(5, 40)
                ]
            )
        };
        
        var userId = Fixture.ExistingUser.Id;
        var createWorkoutDto = new CreateWorkoutDto(title, description, durationInMinutes, level, null, exerciseDtos);
        var createWorkoutCommand = new CreateWorkoutCommand(createWorkoutDto, userId);
        
        var workout = await Fixture.CreateWorkoutCommandHandler.HandleAsync(createWorkoutCommand);

        var command = new DeleteWorkoutCommand(workout.Response.Id, userId);
        
        // Act
        var result = await Fixture.DeleteWorkoutCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be(ResponseMessages.WorkoutDeleted);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        var title = "Example Title";
        var description = "Example Description";
        var durationInMinutes = 35;
        var level = DifficultyLevel.Advanced;
        var exerciseDtos = Array.Empty<CreateExerciseDto>();
        
        var userId = Fixture.ExistingUser.Id;
        var createWorkoutDto = new CreateWorkoutDto(title, description, durationInMinutes, level, null, exerciseDtos);
        var createWorkoutCommand = new CreateWorkoutCommand(createWorkoutDto, userId);
        
        var workout = await Fixture.CreateWorkoutCommandHandler.HandleAsync(createWorkoutCommand);

        var notExistingUserId = Guid.Empty.ToString();
        var command = new DeleteWorkoutCommand(workout.Response.Id, notExistingUserId);
        
        // Act
        var result = await Fixture.DeleteWorkoutCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.UserNotFound);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenWorkoutNotFound()
    {
        // Arrange
        var userId = Fixture.ExistingUser.Id;
        var notExistingWorkoutId = Guid.Empty;
        
        var command = new DeleteWorkoutCommand(notExistingWorkoutId, userId);
        
        // Act
        var result = await Fixture.DeleteWorkoutCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.WorkoutNotFound);
    }
}