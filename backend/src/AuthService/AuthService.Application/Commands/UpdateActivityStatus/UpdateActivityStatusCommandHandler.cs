using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.UpdateActivityStatus;

public class UpdateActivityStatusCommandHandler : ICommandHandler<UpdateActivityStatusCommand, DateTime>
{
    private readonly AuthDbContext _context;

    public UpdateActivityStatusCommandHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<DateTime, Error>> HandleAsync(UpdateActivityStatusCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user == null)
        {
            return Result<DateTime>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        user.UpdateLastSeen();

        await _context.SaveChangesAsync();
        
        return Result<DateTime>.Success(user.LastSeenAt);
    }
}