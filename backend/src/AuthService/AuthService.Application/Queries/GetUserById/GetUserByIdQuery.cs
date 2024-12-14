using Shared.Application.Abstractions;

namespace AuthService.Application.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IQuery;