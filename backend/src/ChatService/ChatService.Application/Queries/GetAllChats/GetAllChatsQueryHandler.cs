using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.GetAllChats;

public class GetAllChatsQueryHandler : IQueryHandler<GetAllChatsQuery, List<Chat>>
{
    private readonly ChatDbContext _context;

    public GetAllChatsQueryHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<Chat>, Error>> HandleAsync(GetAllChatsQuery query)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.UserId);

        if (user is null)
        {
            return Result<List<Chat>>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var chats = await _context.Chats
            .Where(c => c.UserChats.Any(uc => uc.UserId == user.Id))
            .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
            .ToListAsync();

        return Result<List<Chat>>.Success(chats);
    }
}