using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class ExerciseTests
{
    [Fact]
    public void Create_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var name = "Example Exercise";
        var level = DifficultyLevel.Beginner;
        var userId = Guid.NewGuid().ToString();
        
        // Act
        var exercise = Exercise.Create(name, level, userId);

        // Assert
        exercise.Name.Should().Be(name);
        exercise.Level.Should().Be(level);
        exercise.UserId.Should().Be(userId);
        exercise.Sets.Should().BeEmpty();
    }
}