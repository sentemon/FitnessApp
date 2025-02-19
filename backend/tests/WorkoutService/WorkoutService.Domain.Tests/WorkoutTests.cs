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
        workout.Url.Should().Be("example-title");
    }

    [Fact]
    public void Update_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var title = "Example Title";
        var description = "Example Description";
        var durationInMinutes = 30u;
        var level = DifficultyLevel.Beginner;
        var userId = Guid.NewGuid().ToString();
        
        var workout = Workout.Create(title, description, durationInMinutes, level, userId);
        
        var newTitle = "New Title";
        var newDescription = "New Description";
        var newDurationInMinutes = 45u;
        var newLevel = DifficultyLevel.Intermediate;

        // Act
        workout.Update(newTitle, newDescription, newDurationInMinutes, newLevel);

        // Assert
        workout.Title.Should().Be(newTitle);
        workout.Description.Should().Be(newDescription);
        workout.DurationInMinutes.Should().Be(newDurationInMinutes);
        workout.Level.Should().Be(newLevel);
        workout.Url.Should().Be("new-title");
    }

    [Fact]
    public void UpdateWhole_ShouldUpdateWholePropertiesCorrectly()
    {
        // Arrange
        var title = "Example Title";
        var description = "Example Description";
        var durationInMinutes = 30u;
        var level = DifficultyLevel.Beginner;
        var userId = Guid.NewGuid().ToString();
        var workout = Workout.Create(title, description, durationInMinutes, level, userId);

        var newTitle = "New Title";
        var newDescription = "New Description";
        var newDurationInMinutes = 90u;
        var newDifficultyLevel = DifficultyLevel.AllLevels;
        var newExercises = Array.Empty<Exercise>();
        
        // Act
        workout.UpdateWhole(newTitle, newDescription, newDurationInMinutes, newDifficultyLevel, newExercises);
        
        // Assert
        workout.Title.Should().Be(newTitle);
        workout.Description.Should().Be(newDescription);
        workout.DurationInMinutes.Should().Be(newDurationInMinutes);
        workout.Level.Should().Be(newDifficultyLevel);
        workout.Url.Should().Be("new-title");
        workout.WorkoutExercises.Select(we => we.Exercise).Should().BeEquivalentTo(newExercises);
    }

    [Fact]
    public void SetImageUrl_ShouldSetImageUrlCorrectly()
    {
        // Arrange
        var workout = Workout.Create("Example Title", "Example Description", 30u, DifficultyLevel.Beginner, Guid.NewGuid().ToString());
        var imageUrl = "https://example.com/image";

        // Act
        workout.SetImageUrl(imageUrl);
        
        // Assert
        workout.ImageUrl.Should().Be(imageUrl);
    }

    [Fact]
    public void AddExercise_ShouldAddExerciseCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var workout = Workout.Create("Example Title", "Example Description", 30u, DifficultyLevel.Beginner, userId);
        var exercise = Exercise.Create("Example", DifficultyLevel.Advanced, userId);

        // Act
        workout.AddExercise(exercise);

        // Assert
        workout.WorkoutExercises.Select(we => we.Exercise).Should().Contain(exercise);
    }
    
    [Fact]
    public void DeleteExercise_ShouldDeleteExerciseCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var workout = Workout.Create("Example Title", "Example Description", 30u, DifficultyLevel.Beginner, userId);
        var exercise = Exercise.Create("Example", DifficultyLevel.Advanced, userId);
        workout.AddExercise(exercise);

        // Act
        workout.DeleteExercise(exercise);

        // Assert
        workout.WorkoutExercises.Select(we => we.Exercise).Should().NotContain(exercise);
    }
}