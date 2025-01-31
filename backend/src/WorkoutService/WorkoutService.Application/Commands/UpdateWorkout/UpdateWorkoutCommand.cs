using WorkoutService.Application.DTOs;
using ICommand = Shared.Application.Abstractions.ICommand;

namespace WorkoutService.Application.Commands.UpdateWorkout;

public record UpdateWorkoutCommand(UpdateWorkoutDto UpdateWorkoutDto) : ICommand;