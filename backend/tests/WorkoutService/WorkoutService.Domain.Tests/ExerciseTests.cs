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

    [Fact]
    public void AddSet_ShouldAddSetCorrectly()
    {
        // Arrange
        var name = "Example Exercise";
        var level = DifficultyLevel.Beginner;
        var userId = Guid.NewGuid().ToString();
        var exercise = Exercise.Create(name, level, userId);
        var set = Set.Create(35, 40, exercise.Id);
        
        // Act
        exercise.AddSet(set);
        
        // Assert
        exercise.Sets.Count.Should().Be(1);
        exercise.Sets.Should().Contain(set);
    }
    
    [Fact]
    public void DeleteSet_ShouldDeleteSetCorrectly()
    {
        // Arrange
        var name = "Example Exercise";
        var level = DifficultyLevel.Beginner;
        var userId = Guid.NewGuid().ToString();
        var exercise = Exercise.Create(name, level, userId);
        var set = Set.Create(35, 40, exercise.Id);
        exercise.AddSet(set);
        
        // Act
        exercise.DeleteSet(set);
        
        // Assert
        exercise.Sets.Should().BeEmpty();
        exercise.Sets.Should().NotContain(set);
    }
}