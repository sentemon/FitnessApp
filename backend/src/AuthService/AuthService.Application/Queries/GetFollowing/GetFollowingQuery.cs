using Shared.Application.Abstractions;

namespace AuthService.Application.Queries.GetFollowing;

public record GetFollowingQuery(string UserId) : IQuery;