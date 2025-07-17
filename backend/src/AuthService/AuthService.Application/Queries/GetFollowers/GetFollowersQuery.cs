using Shared.Application.Abstractions;

namespace AuthService.Application.Queries.GetFollowers;

public record GetFollowersQuery(string UserId) : IQuery;