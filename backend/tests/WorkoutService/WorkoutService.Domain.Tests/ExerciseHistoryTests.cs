using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class ExerciseHistoryTests
{
    [Fact]
    public void Create_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var workoutId = Workout.Create("Example", "Example", 35, DifficultyLevel.Beginner, userId).Id;
        var exercise = Exercise.Create("Example", DifficultyLevel.Beginner, userId);
        var workoutHistory = WorkoutHistory.Create(35, workoutId, userId);
        
        // Act
        var exerciseHistory = ExerciseHistory.Create(workoutHistory.Id, exercise.Id);

        // Assert
        exerciseHistory.SetHistories.Should().BeEmpty();
    }

    [Fact]
    public void AddSetHistory_ShouldAddSetHistoryCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var workoutId = Workout.Create("Example", "Example", 35, DifficultyLevel.Beginner, userId).Id;
        var exercise = Exercise.Create("Example", DifficultyLevel.Beginner, userId);
        var workoutHistory = WorkoutHistory.Create(35, workoutId, userId);
        var exerciseHistory = ExerciseHistory.Create(workoutHistory.Id, exercise.Id);

        var setHistory = SetHistory.Create(exerciseHistory.Id, 45, 2);
        
        // Act
        exerciseHistory.AddSetHistory(setHistory);
        
        // Assert
        exerciseHistory.SetHistories.Count.Should().Be(1);
        exerciseHistory.SetHistories.Should().Contain(setHistory);
    }
    
    [Fact]
    public void DeleteSetHistory_ShouldDeleteSetHistoryCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var workoutId = Workout.Create("Example", "Example", 35, DifficultyLevel.Beginner, userId).Id;
        var exercise = Exercise.Create("Example", DifficultyLevel.Beginner, userId);
        var workoutHistory = WorkoutHistory.Create(35, workoutId, userId);
        var exerciseHistory = ExerciseHistory.Create(workoutHistory.Id, exercise.Id);

        var setHistory = SetHistory.Create(exerciseHistory.Id, 45, 2);
        exerciseHistory.AddSetHistory(setHistory);
        
        // Act
        exerciseHistory.DeleteSet(setHistory);
        
        // Assert
        exerciseHistory.SetHistories.Should().BeEmpty();
        exerciseHistory.SetHistories.Should().NotContain(setHistory);
    }
}