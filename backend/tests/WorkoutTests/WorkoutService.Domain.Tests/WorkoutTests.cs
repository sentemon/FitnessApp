using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class WorkoutTests
{
    [Fact]
    public void Create_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var title = "Example Title";
        var description = "Example Description";
        var durationInMinutes = 30u;
        var level = DifficultyLevel.Beginner;
        var userId = Guid.NewGuid().ToString();

        // Act
        var workout = Workout.Create(title, description, durationInMinutes, level, userId);

        // Assert
        workout.Title.Should().Be(title);
        workout.Description.Should().Be(description);
        workout.DurationInMinutes.Should().Be(durationInMinutes);
        workout.Level.Should().Be(level);
        workout.UserId.Should().Be(userId);
        workout.IsCustom.Should().BeTrue();
    }
}