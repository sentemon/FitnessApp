using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, string>
{
    private readonly IUserService _userService;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(IUserService userService, ILogger<ResetPasswordCommandHandler> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(ResetPasswordCommand command)
    {
        if (command.UserId == null)
        {
            _logger.LogWarning("Reset password attempt with null UserId.");
            return Result<string>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var result = await _userService.ResetPasswordAsync(command.UserId, command.NewPassword);

        if (!result)
        {
            _logger.LogError("Error during password reset for UserId: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.ErrorDuringResetPassword));
        }
        
        _logger.LogInformation("Password reset successfully for UserId: {UserId}", command.UserId);
        return Result<string>.Success(ResponseMessages.PasswordUpdatedSuccessfully);
    }
}