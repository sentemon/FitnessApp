using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.GetAllChats;

public class GetAllChatsQueryHandler : IQueryHandler<GetAllChatsQuery, List<Chat>>
{
    private readonly ChatDbContext _context;
    private readonly ILogger<GetAllChatsQueryHandler> _logger;

    public GetAllChatsQueryHandler(ChatDbContext context, ILogger<GetAllChatsQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<List<Chat>, Error>> HandleAsync(GetAllChatsQuery query)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == query.UserId);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", query.UserId);
            return Result<List<Chat>>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var sortedChats = await _context.Chats
            .AsNoTracking()
            .Where(c => c.UserChats.Any(uc => uc.UserId == user.Id))
            .Select(c => new
            {
                Chat = c,
                LastMessageTime = c.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => m.SentAt)
                    .FirstOrDefault()
            })
            .OrderByDescending(x => x.LastMessageTime)
            .Select(x => x.Chat)
            .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
            .ToListAsync();

        return Result<List<Chat>>.Success(sortedChats);
    }
}