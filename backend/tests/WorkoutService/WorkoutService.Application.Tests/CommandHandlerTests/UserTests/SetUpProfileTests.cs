using FluentAssertions;
using WorkoutService.Application.Commands.SetUpProfile;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.UserTests;

public class SetUpProfileTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldSetUpProfile_WhenDataIsValid()
    {
        // Arrange
        var weight = 69.63f;
        var height = 175.6f;
        var dateOfBirth = new DateTime(2000, 4, 12).ToUniversalTime();
        var goal = Goal.GainMuscle;
        var activityLevel = ActivityLevel.ModeratelyActive;
        var favoriteWorkoutTypes = new[] { WorkoutType.StrengthTraining, WorkoutType.Other };
        var userId = Fixture.ExistingUser.Id;

        var setUpProfileDto = new SetUpProfileDto(weight, height, dateOfBirth, goal, activityLevel, favoriteWorkoutTypes);

        var command = new SetUpProfileCommand(setUpProfileDto, userId);

        // Act
        var result = await Fixture.SetUpProfileCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Should().Be(ResponseMessages.ProfileSetUp);
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenDataIsInvalid()
    {
        // Arrange
        var weight = -69.63f;
        var height = -175.6f;
        var dateOfBirth = new DateTime(3000, 4, 12).ToUniversalTime();
        var goal = (Goal)34;
        var activityLevel = (ActivityLevel)43;
        var favoriteWorkoutTypes = new[] { WorkoutType.StrengthTraining, WorkoutType.Other };
        var userId = Fixture.ExistingUser.Id;

        var setUpProfileDto = new SetUpProfileDto(weight, height, dateOfBirth, goal, activityLevel, favoriteWorkoutTypes);

        var command = new SetUpProfileCommand(setUpProfileDto, userId);

        // Act
        var result = await Fixture.SetUpProfileCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be("Weight must be greater than 0.; Height must be greater than 0.; Date of Birth must be in the past.; Invalid goal specified.; Invalid activity level specified.");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        var weight = 69.63f;
        var height = 175.6f;
        var dateOfBirth = new DateTime(2000, 4, 12).ToUniversalTime();
        var goal = Goal.GainMuscle;
        var activityLevel = ActivityLevel.ModeratelyActive;
        var favoriteWorkoutTypes = new[] { WorkoutType.StrengthTraining, WorkoutType.Other };
        var userId = Guid.Empty.ToString();

        var setUpProfileDto = new SetUpProfileDto(weight, height, dateOfBirth, goal, activityLevel, favoriteWorkoutTypes);

        var command = new SetUpProfileCommand(setUpProfileDto, userId);

        // Act
        var result = await Fixture.SetUpProfileCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.UserNotFound);
    }
}