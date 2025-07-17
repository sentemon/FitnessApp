using Shared.Application.Abstractions;

namespace AuthService.Application.Queries.IsFollowing;

public record IsFollowingQuery(string TargetUserId, string? UserId) : IQuery;