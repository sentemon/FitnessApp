using Shared.Application.Abstractions;

namespace WorkoutService.Application.Queries.ProfileSetUp;

public record ProfileSetUpQuery(string? UserId) : IQuery;