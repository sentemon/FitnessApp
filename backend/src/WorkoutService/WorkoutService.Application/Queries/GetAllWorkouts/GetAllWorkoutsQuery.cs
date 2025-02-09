using Shared.Application.Abstractions;

namespace WorkoutService.Application.Queries.GetAllWorkouts;

public record GetAllWorkoutsQuery(string? UserId) : IQuery;