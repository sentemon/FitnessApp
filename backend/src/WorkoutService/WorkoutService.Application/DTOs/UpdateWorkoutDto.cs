using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record UpdateWorkoutDto(
    string Id,
    string Title,
    string Description,
    uint DurationInMinutes,
    DifficultyLevel Level
);