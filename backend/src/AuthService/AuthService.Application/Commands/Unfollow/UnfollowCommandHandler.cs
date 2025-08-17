using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Unfollow;

public class UnfollowCommandHandler : ICommandHandler<UnfollowCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly ILogger<UnfollowCommandHandler> _logger;

    public UnfollowCommandHandler(AuthDbContext context, ILogger<UnfollowCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
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
            _logger.LogWarning("Unfollow attempt with non-existing follow relationship: UserId={UserId}, TargetUserId={TargetUserId}", command.UserId, command.TargetUserId);
            return Result<string>.Failure(new Error("Follow relationship not found."));
        }

        _context.Follows.Remove(follow);
        
        follow.Follower.UnfollowUser();
        follow.Following.RemoveFollower();
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User {UserId} unfollowed user {TargetUserId} successfully.", command.UserId, command.TargetUserId);
        return Result<string>.Success("User unfollowed successfully");
    }
}