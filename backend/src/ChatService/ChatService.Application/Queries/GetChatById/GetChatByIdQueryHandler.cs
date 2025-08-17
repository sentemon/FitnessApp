using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.GetChatById;

public class GetChatByIdQueryHandler : IQueryHandler<GetChatByIdQuery, Chat>
{
    private readonly ChatDbContext _context;
    private readonly ILogger<GetChatByIdQueryHandler> _logger;

    public GetChatByIdQueryHandler(ChatDbContext context, ILogger<GetChatByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<Chat, Error>> HandleAsync(GetChatByIdQuery query)
    {
        var chat = await _context.Chats
            .AsNoTracking()
            .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
            .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
            .FirstOrDefaultAsync(c => c.Id == query.ChatId);

        if (chat is null)
        {
            _logger.LogWarning("Chat with ID {ChatId} not found.", query.ChatId);
            return Result<Chat>.Failure(new Error(ResponseMessages.ChatNotFound));
        }

        return Result<Chat>.Success(chat);
    }
}