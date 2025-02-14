using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.DeleteSetHistory;

public record DeleteSetHistoryCommand(Guid Id, Guid ExerciseHistoryId) : ICommand;