using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class WorkoutExerciseTests
{
    [Fact]
    public void Constructor_ShouldCreateWorkoutExercise()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var workout = Workout.Create("Example", "Example", 45u, DifficultyLevel.Advanced, userId);
        var exercise = Exercise.Create("Example", DifficultyLevel.Beginner, userId);

        // Act
        var workoutExercise = new WorkoutExercise(workout, exercise);

        // Assert
        workoutExercise.Workout.Should().Be(workout);
        workoutExercise.Exercise.Should().Be(exercise);
    }
}