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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.TargetUserId);

        if (user is null || targetUser is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        user.UnfollowUser(targetUser);
        await _context.SaveChangesAsync();
        
        return Result<string>.Success("User unfollowed successfully");
    }
}