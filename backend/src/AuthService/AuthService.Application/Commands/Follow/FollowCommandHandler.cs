using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
    
namespace AuthService.Application.Commands.Follow;

public class FollowCommandHandler : ICommandHandler<FollowCommand, string>
{
    private readonly AuthDbContext _context;

    public FollowCommandHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(FollowCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.TargetUserId);

        if (user is null || targetUser is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var alreadyFollowing = await _context.Follows
            .AnyAsync(f => f.FollowerId == user.Id && f.FollowingId == targetUser.Id);

        if (alreadyFollowing)
        {
            return Result<string>.Failure(new Error("Already following this user."));
        }

        var follow = Domain.Entities.Follow.Create(user.Id, targetUser.Id);
        _context.Follows.Add(follow);

        await _context.SaveChangesAsync();

        return Result<string>.Success("User followed successfully");
    }
}