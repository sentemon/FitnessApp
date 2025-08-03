using AuthService.Domain.Constants;
using AuthService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO.Messages;

namespace AuthService.Application.Commands.UpdateActivityStatus;

public class UpdateActivityStatusCommandHandler : ICommandHandler<UpdateActivityStatusCommand, DateTime>
{
    private readonly AuthDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateActivityStatusCommandHandler(AuthDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
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
        
        await _publishEndpoint.Publish(new ActivityStatusUpdatedEventMessage(
            user.Id,
            user.LastSeenAt
        ));
        
        return Result<DateTime>.Success(user.LastSeenAt);
    }
}