using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record ExerciseDto(
    Guid Id,
    string Name, 
    DifficultyLevel Level, 
    SetDto[] Sets
);