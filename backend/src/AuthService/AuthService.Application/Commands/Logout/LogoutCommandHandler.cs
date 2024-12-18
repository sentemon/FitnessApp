using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Logout;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand, string>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IResult<string, Error>> HandleAsync(LogoutCommand command)
    {
        var result = await _authService.LogoutAsync(command.RefreshToken);

        if (!result)
        {
            return Result<string>.Failure(new Error(ResponseMessages.ErrorDuringLogout));
        }
        
        return Result<string>.Success(ResponseMessages.LoggedOutSuccessfully);
    }
}