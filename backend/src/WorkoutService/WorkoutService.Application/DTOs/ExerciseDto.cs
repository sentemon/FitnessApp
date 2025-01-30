using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record ExerciseDto(
    string Name, 
    DifficultyLevel Level, 
    SetDto[] Sets
);