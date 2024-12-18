using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, string>
{
    private readonly IUserService _userService;

    public ResetPasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResult<string, Error>> HandleAsync(ResetPasswordCommand command)
    {
        var result = await _userService.ResetPasswordAsync(command.UserId, command.NewPassword);

        if (!result)
        {
            return Result<string>.Failure(new Error(ResponseMessages.ErrorDuringResetPassword));
        }
        
        return Result<string>.Success(ResponseMessages.PasswordUpdatedSuccessfully);
    }
}