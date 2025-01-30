using HotChocolate.Types;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record WorkoutDto(
    string Title,
    string Description,
    int Time,
    DifficultyLevel Level,
    IFile? File,
    ExerciseDto[] Exercises
);