using Shared.Application.Abstractions;

namespace WorkoutService.Application.Queries.GetAllWorkoutHistories;

public record GetAllWorkoutHistoriesQuery(string? UserId) : IQuery;