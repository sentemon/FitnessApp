using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
    
namespace AuthService.Application.Commands.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly ILogger<FollowCommandHandler> _logger;

    public FollowCommandHandler(AuthDbContext context, ILogger<FollowCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(FollowCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.TargetUserId);

        if (user is null || targetUser is null)
        {
            _logger.LogWarning("Follow attempt with non-existing user: UserId={UserId}, TargetUserId={TargetUserId}", command.UserId, command.TargetUserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var alreadyFollowing = await _context.Follows
            .AnyAsync(f => f.FollowerId == user.Id && f.FollowingId == targetUser.Id);

        if (alreadyFollowing)
        {
            _logger.LogWarning("Follow attempt with already existing follow: UserId={UserId}, TargetUserId={TargetUserId}", command.UserId, command.TargetUserId);
            return Result<string>.Failure(new Error("Already following this user."));
        }

        var follow = Domain.Entities.Follow.Create(user.Id, targetUser.Id);
        _context.Follows.Add(follow);
        
        user.FollowUser();
        targetUser.AddFollower();

        await _context.SaveChangesAsync();
        _logger.LogInformation("User {UserId} followed user {TargetUserId} successfully.", command.UserId, command.TargetUserId);

        return Result<string>.Success("User followed successfully");
    }
}