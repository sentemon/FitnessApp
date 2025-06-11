using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record WorkoutDto(
    Guid Id,
    string Title,
    string Description,
    uint DurationInMinutes,
    DifficultyLevel Level,
    bool IsCustom,
    string Url,
    string ImageUrl,
    ExerciseDto[] Exercises
);