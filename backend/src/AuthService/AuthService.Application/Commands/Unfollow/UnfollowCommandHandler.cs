using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Unfollow;

public class UnfollowCommandHandler : ICommandHandler<UnfollowCommand, string>
{
    private readonly AuthDbContext _context;

    public UnfollowCommandHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(UnfollowCommand command)
    {
        var follow = await _context.Follows
            .Include(f => f.Following)
            .Include(f => f.Follower)
            .FirstOrDefaultAsync(f =>
                f.FollowerId == command.UserId &&
                f.FollowingId == command.TargetUserId);

        if (follow is null)
        {
            return Result<string>.Failure(new Error("Follow relationship not found."));
        }

        _context.Follows.Remove(follow);
        
        follow.Follower.UnfollowUser();
        follow.Following.RemoveFollower();
        
        await _context.SaveChangesAsync();
        
        return Result<string>.Success("User unfollowed successfully");
    }
}