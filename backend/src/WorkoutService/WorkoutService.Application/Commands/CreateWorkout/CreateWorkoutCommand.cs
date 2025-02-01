using Shared.Application.Abstractions;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Application.Commands.CreateWorkout;

public record CreateWorkoutCommand(CreateWorkoutDto CreateWorkoutDto, string? UserId) : ICommand;