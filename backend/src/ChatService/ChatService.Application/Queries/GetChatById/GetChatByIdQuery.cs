using Shared.Application.Abstractions;

namespace ChatService.Application.Queries.GetChatById;

public record GetChatByIdQuery(Guid ChatId) : IQuery;