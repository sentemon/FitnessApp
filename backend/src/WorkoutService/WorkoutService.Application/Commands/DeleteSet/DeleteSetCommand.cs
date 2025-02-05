using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.DeleteSet;

public record DeleteSetCommand(Guid Id, Guid ExerciseId) : ICommand;