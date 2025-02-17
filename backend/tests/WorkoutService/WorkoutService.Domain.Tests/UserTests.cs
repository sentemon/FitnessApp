using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class UserTests
{
    [Fact]
    public async Task SetUpProfile_ShouldSetUpProfileCorrectly()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid().ToString(), "Example", "Example", "example", string.Empty);
        
        var weight = 70.34f;
        var height = 187.4f;
        var currentGoal = Goal.GainMuscle;
        var activityLevel = ActivityLevel.VeryActive;
        var favoriteWorkoutTypes = new[] { WorkoutType.Cardio, WorkoutType.Other };
        var dateOfBirth = new DateTime(2000, 3, 23);

        // Act
        user.SetUpProfile(weight, height, currentGoal, activityLevel, favoriteWorkoutTypes, dateOfBirth);

        // Assert
        user.Weight.Should().Be(weight);
        user.Height.Should().Be(height);
        user.CurrentGoal.Should().Be(currentGoal);
        user.ActivityLevel.Should().Be(activityLevel);
        user.FavoriteWorkoutTypes.Should().Equal(favoriteWorkoutTypes);
        user.DateOfBirth.Should().Be(dateOfBirth);
    }
}