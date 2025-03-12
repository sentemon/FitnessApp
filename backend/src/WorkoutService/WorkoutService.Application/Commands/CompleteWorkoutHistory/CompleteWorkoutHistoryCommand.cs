using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.CompleteWorkoutHistory;

public record CompleteWorkoutHistoryCommand(Guid Id, uint DurationInMinutes) : ICommand;