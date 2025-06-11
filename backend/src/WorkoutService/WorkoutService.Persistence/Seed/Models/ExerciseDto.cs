using WorkoutService.Domain.Enums;

namespace WorkoutService.Persistence.Seed.Models;

public record ExerciseDto(
    string Name,
    DifficultyLevel Level,
    SetDto[] Sets
);