using FluentAssertions;
using WorkoutService.Application.Commands.AddSetHistory;
using WorkoutService.Domain.Constants;
using Xunit;

namespace WorkoutService.Application.Tests.CommandHandlerTests.SetHistoryTests;

public class AddSetHistoryTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task HandleAsync_ShouldAddSetHistory_WhenDataIsValid()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var exerciseHistoryId = Fixture.ExistingExerciseHistory.Id;

        var command = new AddSetHistoryCommand(reps, weight, exerciseHistoryId);
        
        // Act
        var result = await Fixture.AddSetHistoryCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Response.Reps.Should().Be(reps);
        result.Response.Weight.Should().Be(weight);
    }
    
    
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenExerciseHistoryNotFound()
    {
        // Arrange
        var reps = 45u;
        var weight = 5;
        var notExistingExerciseHistoryId = Guid.Empty;

        var command = new AddSetHistoryCommand(reps, weight, notExistingExerciseHistoryId);
        
        // Act
        var result = await Fixture.AddSetHistoryCommandHandler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(ResponseMessages.ExerciseHistoryNotFound);
    }
}