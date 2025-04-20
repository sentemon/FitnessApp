using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.GetChatById;

public class GetChatByIdQueryHandler : IQueryHandler<GetChatByIdQuery, Chat>
{
    private readonly ChatDbContext _context;

    public GetChatByIdQueryHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<Chat, Error>> HandleAsync(GetChatByIdQuery query)
    {
        var chat = await _context.Chats
            .Include(c => c.Messages)
            .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
            .FirstOrDefaultAsync(c => c.Id == query.ChatId);

        if (chat is null)
        {
            return Result<Chat>.Failure(new Error(ResponseMessages.ChatNotFound));
        }

        return Result<Chat>.Success(chat);
    }
}