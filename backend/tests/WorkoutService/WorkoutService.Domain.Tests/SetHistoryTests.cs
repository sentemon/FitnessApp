using FluentAssertions;
using WorkoutService.Domain.Entities;
using Xunit;

namespace WorkoutService.Domain.Tests;

public class SetHistoryTests
{
    [Fact]
    public void MarkAsCompleted_ShouldMarkPropertyCompletedAsTrue()
    {
        // Arrange
        var reps = 12u;
        var weight = 30;
        var exerciseHistoryId = ExerciseHistory.Create(Guid.NewGuid(), Guid.NewGuid()).Id;
        
        var setHistory = SetHistory.Create(exerciseHistoryId, reps, weight);
        
        // Act
        setHistory.MarkAsCompleted();
        
        // Assert
        setHistory.Completed.Should().BeTrue();
    }
    
    [Fact]
    public void MarkAsUncompleted_ShouldMarkPropertyCompletedAsFalse()
    {
        // Arrange
        var reps = 12u;
        var weight = 30;
        var exerciseHistoryId = ExerciseHistory.Create(Guid.NewGuid(), Guid.NewGuid()).Id;
        
        var setHistory = SetHistory.Create(exerciseHistoryId, reps, weight);
        
        // Act
        setHistory.MarkAsUncompleted();
        
        // Assert
        setHistory.Completed.Should().BeFalse();
    }
}