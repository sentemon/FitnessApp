using WorkoutService.Domain.Enums;

namespace WorkoutService.Persistence.Seed.Models;

public record WorkoutDto(
    string Title,
    string Description,
    uint DurationInMinutes,
    DifficultyLevel Level,
    List<ExerciseDto> Exercises
);