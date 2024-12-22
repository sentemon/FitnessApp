using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, KeycloakTokenResponse>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IResult<KeycloakTokenResponse, Error>> HandleAsync(LoginCommand command)
    {
        var token = await _authService.LoginAsync(command.LoginDto.Username, command.LoginDto.Password);

        return Result<KeycloakTokenResponse>.Success(token);
    }
}