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
        var user = await _context.Users
            .Include(u => u.UserChats)
            .ThenInclude(uc => uc.Chat)
            .FirstOrDefaultAsync(u => u.Id == query.UserId);

        if (user is null)
        {
            return Result<List<Chat>>.Failure(new Error("User not found."));
        }

        var chats = user.UserChats.Select(uc => uc.Chat).ToList();

        return Result<List<Chat>>.Success(chats);
    }
}