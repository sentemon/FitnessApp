using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class SetTests
{
    [Fact]
    public void Create_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var reps = 12u;
        var weight = 30;
        var exerciseId = Exercise.Create("Example Exercise", DifficultyLevel.Beginner, Guid.NewGuid().ToString()).Id;
        
        // Act
        var set = Set.Create(reps, weight, exerciseId);

        // Assert
        set.Reps.Should().Be(reps);
        set.Weight.Should().Be(weight);
        set.ExerciseId.Should().Be(exerciseId);
    }

    // [Fact]
    // public void MarkAsCompleted_ShouldMarkPropertyCompletedAsTrue()
    // {
    //     // Arrange
    //     var reps = 12u;
    //     var weight = 30;
    //     var exerciseId = Exercise.Create("Example Exercise", DifficultyLevel.Beginner, Guid.NewGuid().ToString()).Id;
    //     
    //     var set = Set.Create(reps, weight, exerciseId);
    //     
    //     // Act
    //     set.MarkAsCompleted();
    //     
    //     // Assert
    //     set.Completed.Should().BeTrue();
    // }
    //
    // [Fact]
    // public void MarkAsUncompleted_ShouldMarkPropertyCompletedAsFalse()
    // {
    //     // Arrange
    //     var reps = 12u;
    //     var weight = 30;
    //     var exerciseId = Exercise.Create("Example Exercise", DifficultyLevel.Beginner, Guid.NewGuid().ToString()).Id;
    //     
    //     var set = Set.Create(reps, weight, exerciseId);
    //     set.MarkAsCompleted();
    //     
    //     // Act
    //     set.MarkAsUncompleted();
    //     
    //     // Assert
    //     set.Completed.Should().BeFalse();
    // }
}