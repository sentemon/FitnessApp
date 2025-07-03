using Shared.Application.Abstractions;

namespace ChatService.Application.Queries.SearchUsers;

public record SearchUsersQuery(string Search, string? UserId) : IQuery;