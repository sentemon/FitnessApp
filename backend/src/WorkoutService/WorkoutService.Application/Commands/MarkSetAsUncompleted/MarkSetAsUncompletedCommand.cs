using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.MarkSetAsUncompleted;

public record MarkSetAsUncompletedCommand(Guid Id, Guid ExerciseId) : ICommand;