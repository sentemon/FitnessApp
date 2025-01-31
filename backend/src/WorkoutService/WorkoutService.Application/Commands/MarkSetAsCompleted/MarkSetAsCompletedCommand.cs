using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.MarkSetAsCompleted;

public record MarkSetAsCompletedCommand(Guid Id, Guid ExerciseId) : ICommand;