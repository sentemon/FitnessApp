using Shared.Application.Abstractions;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Application.Commands.CreateWorkout;

public record CreateWorkoutCommand(WorkoutDto WorkoutDto, string UserId) : ICommand;