using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.DeleteWorkout;

public record DeleteWorkoutCommand(Guid WorkoutId, string? UserId) : ICommand;