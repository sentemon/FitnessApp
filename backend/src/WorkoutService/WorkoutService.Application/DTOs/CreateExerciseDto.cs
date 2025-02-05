using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record CreateExerciseDto(
    string Name, 
    DifficultyLevel Level, 
    CreateSetDto[] Sets
);