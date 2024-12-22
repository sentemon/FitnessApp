using Shared.Application.Abstractions;

namespace AuthService.Application.Queries.GetUserByUsername;

public record GetUserByUsernameQuery(string Username) : IQuery;