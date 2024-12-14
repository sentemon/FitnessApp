using AuthService.Infrastructure.Interfaces;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IKeycloakService _keycloakService;

    public LoginCommandHandler(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task<IResult<string, Error>> HandleAsync(LoginCommand command)
    {
        var token = await _keycloakService.LoginAsync(command.LoginDto.Username, command.LoginDto.Password);

        return Result<string>.Success(token.AccessToken);
    }
}