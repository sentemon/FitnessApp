using Shared.Application.Abstractions;

namespace ChatService.Application.Queries.GetLastMessage;

public record GetLastMessageQuery(Guid ChatId, string? UserId) : IQuery;