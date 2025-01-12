using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO;
using Shared.DTO.Messages;

namespace AuthService.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, string>
{
    private readonly IUserService _userService;
    private readonly AuthDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateUserCommandHandler(IUserService userService, AuthDbContext context, IPublishEndpoint publishEndpoint)
    {
        _userService = userService;
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IResult<string, Error>> HandleAsync(UpdateUserCommand command)
    {
        if (command.Id == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var updatedUser = await _userService.UpdateAsync(
            command.Id,
            command.UpdateUserDto.FirstName,
            command.UpdateUserDto.LastName,
            command.UpdateUserDto.Username,
            command.UpdateUserDto.Email
        );

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

        if (user == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        user.Update(
            command.UpdateUserDto.FirstName,
            command.UpdateUserDto.LastName,
            command.UpdateUserDto.Username,
            command.UpdateUserDto.Email
        );
        
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new UserUpdatedEventMessage(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username.Value,
            user.ImageUrl
        ));
        
        return Result<string>.Success(ResponseMessages.UserUpdatedSuccessfully);
    }
}