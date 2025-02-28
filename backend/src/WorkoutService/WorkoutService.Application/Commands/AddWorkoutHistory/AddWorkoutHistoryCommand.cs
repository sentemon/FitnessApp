using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.AddWorkoutHistory;

public record AddWorkoutHistoryCommand(Guid WorkoutId, string? UserId) : ICommand;