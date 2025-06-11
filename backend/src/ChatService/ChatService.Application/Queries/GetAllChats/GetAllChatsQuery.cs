using Shared.Application.Abstractions;

namespace ChatService.Application.Queries.GetAllChats;

public record GetAllChatsQuery(string? UserId) : IQuery;