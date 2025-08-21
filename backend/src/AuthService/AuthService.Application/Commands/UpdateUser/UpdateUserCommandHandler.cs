using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.Application.Extensions;
using Shared.DTO;
using Shared.DTO.Messages;

namespace AuthService.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, string>
{
    private readonly IUserService _userService;
    private readonly AuthDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IUserService userService, AuthDbContext context, IPublishEndpoint publishEndpoint, ILogger<UpdateUserCommandHandler> logger)
    {
        _userService = userService;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(UpdateUserCommand command)
    {
        if (command.UserId == null)
        {
            _logger.LogWarning("Update user attempt with null UserId.");
            return Result<string>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var updatedUser = await _userService.UpdateAsync(
            command.UserId,
            command.UpdateUserDto.FirstName,
            command.UpdateUserDto.LastName,
            command.UpdateUserDto.Username,
            command.UpdateUserDto.Email
        );

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

        if (user == null)
        {
            _logger.LogWarning("Update user attempt with non-existing user ID: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        user.Update(
            command.UpdateUserDto.FirstName,
            command.UpdateUserDto.LastName,
            command.UpdateUserDto.Username,
            command.UpdateUserDto.Email
        );
        
        await _context.SaveChangesAsync();
        
        await _publishEndpoint.Publish(new UserSetImageEventMessage(
            FileExtensions.ReadFully(command.UpdateUserDto.Image?.OpenReadStream()),
            FileExtensions.GetContentType(command.UpdateUserDto.Image),
            user.Id
        ));
        
        await _publishEndpoint.Publish(new UserUpdatedEventMessage(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username.Value,
            user.ImageUrl
        ));
        
        _logger.LogInformation("User {UserId} updated successfully.", user.Id);
        return Result<string>.Success(ResponseMessages.UserUpdatedSuccessfully);
    }
}