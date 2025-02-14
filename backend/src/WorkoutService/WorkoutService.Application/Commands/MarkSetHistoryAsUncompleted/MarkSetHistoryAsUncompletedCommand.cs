using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.MarkSetHistoryAsUncompleted;

public record MarkSetHistoryAsUncompletedCommand(Guid Id, Guid ExerciseHistoryId) : ICommand;