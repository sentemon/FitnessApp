using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.GetLastMessage;

public class GetLastMessageQueryHandler : IQueryHandler<GetLastMessageQuery, Message>
{
    private readonly ChatDbContext _context;
    private readonly ILogger<GetLastMessageQueryHandler> _logger;

    public GetLastMessageQueryHandler(ChatDbContext context, ILogger<GetLastMessageQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<Message, Error>> HandleAsync(GetLastMessageQuery query)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.UserId);
        if (user is null)   
        {
            _logger.LogWarning("User with ID {UserId} not found.", query.UserId);
            return Result<Message>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var chat = await _context.Chats
            .AsNoTracking()
            .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(c => c.Id == query.ChatId);
        
        if (chat is null)
        {
            _logger.LogWarning("Chat with ID {ChatId} not found for user {UserId}.", query.ChatId, query.UserId);
            return Result<Message>.Failure(new Error(ResponseMessages.ChatNotFound));
        }
        
        var lastMessage = chat.Messages.MaxBy(m => m.SentAt);
        
        if (lastMessage is null)
        {
            _logger.LogWarning("No messages found in chat with ID {ChatId} for user {UserId}.", query.ChatId, query.UserId);
            return Result<Message>.Failure(new Error(ResponseMessages.NoMessagesInChat));
        }

        return Result<Message>.Success(lastMessage);
    }
}