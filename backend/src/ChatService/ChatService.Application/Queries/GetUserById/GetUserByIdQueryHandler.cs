using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly ChatDbContext _context;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(ChatDbContext context, ILogger<GetUserByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<User, Error>> HandleAsync(GetUserByIdQuery query)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == query.UserId);
        
        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", query.UserId);
            return Result<User>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        return Result<User>.Success(user);
    }
}