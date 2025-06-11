using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.AddSetHistory;

public record AddSetHistoryCommand(uint Reps, int Weight, Guid ExerciseHistoryId) : ICommand;