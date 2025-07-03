using Shared.Application.Abstractions;

namespace AuthService.Application.Queries.SearchUsers;

public record SearchUsersQuery(string Search, string? UserId) : IQuery;