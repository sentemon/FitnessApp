using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.SetDeleted;

public record SetDeletedCommand(Guid Id, Guid ExerciseId) : ICommand;