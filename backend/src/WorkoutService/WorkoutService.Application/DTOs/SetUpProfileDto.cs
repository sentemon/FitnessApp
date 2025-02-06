using WorkoutService.Domain.Enums;

namespace WorkoutService.Application.DTOs;

public record SetUpProfileDto(
    float Weight,
    float Height,
    DateTime? DateOfBirth,
    Goal Goal,
    ActivityLevel ActivityLevel,
    WorkoutType[] WorkoutTypes
);