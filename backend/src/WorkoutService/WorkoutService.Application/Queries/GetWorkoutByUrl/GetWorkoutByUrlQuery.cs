using Shared.Application.Abstractions;

namespace WorkoutService.Application.Queries.GetWorkoutByUrl;

public record GetWorkoutByUrlQuery(string Url) : IQuery;