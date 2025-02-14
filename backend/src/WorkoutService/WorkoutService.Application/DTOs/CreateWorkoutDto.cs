using HotChocolate.Types;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record CreateWorkoutDto(
    string Title,
    string Description,
    uint DurationInMinutes,
    DifficultyLevel Level,
    IFile? ImageUrl,
    CreateExerciseDto[] Exercises
);