using AuthService.Application.Queries.GetFollowers;
using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetFollowing;

public class GetFollowingQueryHandler : IQueryHandler<GetFollowingQuery, ICollection<User>>
{
    private readonly AuthDbContext _context;
    private readonly ILogger<GetFollowingQueryHandler> _logger;

    public GetFollowingQueryHandler(AuthDbContext context, ILogger<GetFollowingQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<ICollection<User>, Error>> HandleAsync(GetFollowingQuery query)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Username)
            .Include(u => u.Following)
                .ThenInclude(f => f.Following)
            .FirstOrDefaultAsync(u => u.Id == query.UserId);

        if (user is null)
        {
            _logger.LogWarning("Get following attempt with non-existing user ID: {UserId}", query.UserId);
            return Result<ICollection<User>>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var following = user.Following.Select(f => f.Following).ToList();
            
        return Result<ICollection<User>>.Success(following);
    }
}