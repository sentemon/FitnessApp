using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class WorkoutHistoryTests
{
    [Fact]
    public void Create_ShouldCreateWorkoutHistoryCorrectly()
    {
        // Arrange
        var workoutId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        // Act
        var workoutHistory = WorkoutHistory.Create(workoutId, userId);

        // Assert
        workoutHistory.WorkoutId.Should().Be(workoutId);
        workoutHistory.ExerciseHistories.Should().BeEmpty();
        workoutHistory.UserId.Should().Be(userId);
    }

    [Fact]
    public void AddExerciseHistory_ShouldAddExerciseHistoryCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var workoutHistory = WorkoutHistory.Create(Guid.NewGuid(), userId);
        var exercise = Exercise.Create("Example", DifficultyLevel.Intermediate, userId);
        var exerciseHistory = ExerciseHistory.Create(workoutHistory.Id, exercise.Id);
        
        // Act
        workoutHistory.AddExerciseHistory(exerciseHistory);
        
        // Assert
        workoutHistory.ExerciseHistories.Should().Contain(exerciseHistory);
    }
}