using Shared.Application.Abstractions;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Application.Commands.AddSet;

public record AddSetCommand(uint Reps, int Weight, Guid ExerciseId) : ICommand;