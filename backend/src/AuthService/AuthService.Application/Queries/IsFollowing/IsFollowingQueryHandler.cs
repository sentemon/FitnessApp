using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.IsFollowing;

public class IsFollowingQueryHandler : IQueryHandler<IsFollowingQuery, bool>
{
    private readonly AuthDbContext _context;

    public IsFollowingQueryHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<bool, Error>> HandleAsync(IsFollowingQuery query)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.UserId);
        var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.TargetUserId);

        if (user is null || targetUser is null)
        {
            return Result<bool>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var isFollowing = await _context.Follows
            .AnyAsync(f => f.FollowerId == query.UserId && f.FollowingId == query.TargetUserId);
        
        return Result<bool>.Success(isFollowing);
    }
}