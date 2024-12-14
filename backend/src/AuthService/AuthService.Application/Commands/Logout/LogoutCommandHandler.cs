using AuthService.Infrastructure.Interfaces;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Logout;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand, string>
{
    private readonly IKeycloakService _keycloakService;

    public LogoutCommandHandler(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task<IResult<string, Error>> HandleAsync(LogoutCommand command)
    {
        var result = await _keycloakService.LogoutAsync(command.RefreshToken);

        if (!result)
        {
            return Result<string>.Failure(new Error("Something was wrong."));
        }
        
        return Result<string>.Success("You logout successfully.");
    }
}