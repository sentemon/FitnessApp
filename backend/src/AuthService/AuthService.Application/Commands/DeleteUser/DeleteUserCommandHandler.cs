using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO.Messages;

namespace AuthService.Application.Commands.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IUserService _userService;
    private readonly AuthDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteUserCommandHandler(IUserService userService, AuthDbContext context, IPublishEndpoint publishEndpoint)
    {
        _userService = userService;
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IResult<bool, Error>> HandleAsync(DeleteUserCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        
        if (user == null)
        {
            return Result<bool>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        try
        {
            await _userService.DeleteAsync(user.Id);
            _context.Users.Remove(user);
        
            await _context.SaveChangesAsync();
        
            await _publishEndpoint.Publish(new UserDeletedEventMessage(user.Id));
            
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(new Error(e.Message));
        }
        
    }
}