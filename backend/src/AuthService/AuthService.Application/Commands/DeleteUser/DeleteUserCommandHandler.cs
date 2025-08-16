using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO.Messages;

namespace AuthService.Application.Commands.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IUserService _userService;
    private readonly AuthDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(IUserService userService, AuthDbContext context, IPublishEndpoint publishEndpoint, ILogger<DeleteUserCommandHandler> logger)
    {
        _userService = userService;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<IResult<bool, Error>> HandleAsync(DeleteUserCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        
        if (user == null)
        {
            _logger.LogWarning("Delete user attempt with non-existing user ID: {UserId}", command.UserId);
            return Result<bool>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        try
        {
            await _userService.DeleteAsync(user.Id);
            _context.Users.Remove(user);
        
            await _context.SaveChangesAsync();
        
            await _publishEndpoint.Publish(new UserDeletedEventMessage(user.Id));
            
            _logger.LogInformation("User with ID {UserId} deleted successfully.", user.Id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: {Error} while deleting user with ID {UserId}. ", ex.Message, user.Id);
            return Result<bool>.Failure(new Error(ex.Message));
        }
        
    }
}