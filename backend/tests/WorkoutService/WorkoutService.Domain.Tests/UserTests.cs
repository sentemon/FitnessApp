using FluentAssertions;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class UserTests
{
    [Fact]
    public void SetUpProfile_ShouldSetUpProfileCorrectly()
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

    [Fact]
    public void AddWorkout_ShouldAddWorkoutCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var user = User.Create(id, "Example", "Example", "example", string.Empty);
        var workout = Workout.Create("Example Title", "Example Description", 30u, DifficultyLevel.Beginner, id);
        
        // Act
        user.AddWorkout(workout);
        
        // Assert
        user.Workouts.Should().Contain(workout);
    }
    
    [Fact]
    public void DeleteWorkout_ShouldDeleteWorkoutCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var user = User.Create(id, "Example", "Example", "example", string.Empty);
        var workout = Workout.Create("Example Title", "Example Description", 30u, DifficultyLevel.Beginner, id);
        user.AddWorkout(workout);
        
        // Act
        user.DeleteWorkout(workout);
        
        // Assert
        user.Workouts.Should().NotContain(workout);
    }
    
    [Fact]
    public void AddExercise_ShouldAddExerciseCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var user = User.Create(id, "Example", "Example", "example", string.Empty);
        var exercise = Exercise.Create("Example Title", DifficultyLevel.Beginner, id);
        
        // Act
        user.AddExercise(exercise);
        
        // Assert
        user.Exercises.Should().Contain(exercise);
    }

    [Fact]
    public void DeleteExercise_ShouldDeleteExerciseCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var user = User.Create(id, "Example", "Example", "example", string.Empty);
        var exercise = Exercise.Create("Example Title", DifficultyLevel.Beginner, id);
        user.AddExercise(exercise);
        
        // Act
        user.DeleteExercise(exercise);
        
        // Assert
        user.Exercises.Should().NotContain(exercise);
    }
    
    [Fact]
    public void AddWorkoutHistory_ShouldAddWorkoutHistoryCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var user = User.Create(id, "Example", "Example", "example", string.Empty);
        var workoutId = Workout.Create("Example Title", "Example Description", 30u, DifficultyLevel.Beginner, id).Id;
        var workoutHistory = WorkoutHistory.Create(workoutId, id);
        
        // Act
        user.AddWorkoutHistory(workoutHistory);
        
        // Assert
        user.WorkoutHistories.Should().Contain(workoutHistory);
    }
    
    [Fact]
    public void DeleteWorkoutHistory_ShouldDeleteWorkoutHistoryCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var user = User.Create(id, "Example", "Example", "example", string.Empty);
        var workoutId = Workout.Create("Example Title", "Example Description", 30u, DifficultyLevel.Beginner, id).Id;
        var workoutHistory = WorkoutHistory.Create(workoutId, id);
        user.AddWorkoutHistory(workoutHistory);
        
        // Act
        user.DeleteWorkoutHistory(workoutHistory);
        
        // Assert
        user.WorkoutHistories.Should().NotContain(workoutHistory);
    }
}