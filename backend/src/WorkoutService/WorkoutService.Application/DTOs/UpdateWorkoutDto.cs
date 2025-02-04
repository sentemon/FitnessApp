using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record UpdateWorkoutDto(
    Guid Id,
    string Title,
    string Description,
    uint DurationInMinutes,
    DifficultyLevel Level
);