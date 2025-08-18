using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, string>
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(IUserService userService, IAuthService authService, ILogger<ResetPasswordCommandHandler> logger)
    {
        _userService = userService;
        _authService = authService;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(ResetPasswordCommand command)
    {
        if (command.UserId == null)
        {
            _logger.LogWarning("Reset password attempt with null UserId.");
            return Result<string>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var user = await _userService.GetByIdAsync(command.UserId);
        if (user is null)
        {
            _logger.LogWarning("Reset password attempt for non-existing UserId: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var isOldPasswordValid = await _authService.LoginAsync(user.Username.Value, command.OldPassword);
        if (string.IsNullOrEmpty(isOldPasswordValid.AccessToken))
        {
            _logger.LogWarning("Reset password attempt with invalid old password for UserId: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.InvalidOldPassword));
        }

        if (command.NewPassword != command.ConfirmNewPassword)
        {
            _logger.LogWarning("Reset password attempt with mismatched new passwords for UserId: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.PasswordsDoNotMatch));
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