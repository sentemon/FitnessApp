using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, string>
{
    private readonly IUserService _userService;
    private readonly AuthDbContext _context;

    public UpdateUserCommandHandler(IUserService userService, AuthDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(UpdateUserCommand command)
    {
        var updatedUser = await _userService.UpdateUserAsync(
            command.Id,
            command.UpdateUserDto.FirstName,
            command.UpdateUserDto.LastName,
            command.UpdateUserDto.Username,
            command.UpdateUserDto.Email
        );

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

        if (user == null)
        {
            return Result<string>.Failure(new Error("User not found"));
        }
        
        user.Update(
            command.UpdateUserDto.FirstName,
            command.UpdateUserDto.LastName,
            command.UpdateUserDto.Username,
            command.UpdateUserDto.Email
        );
        
        await _context.SaveChangesAsync();
        
        return Result<string>.Success("User updated successfully.");
    }
}