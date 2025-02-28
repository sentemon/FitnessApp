using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.AddWorkoutHistory;

public record AddWorkoutHistoryCommand(uint DurationInMinutes, Guid WorkoutId, string? UserId) : ICommand;