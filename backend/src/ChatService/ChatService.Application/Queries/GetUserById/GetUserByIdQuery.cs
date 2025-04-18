using Shared.Application.Abstractions;

namespace ChatService.Application.Queries.GetUserById;

public record GetUserByIdQuery(string? UserId) : IQuery;