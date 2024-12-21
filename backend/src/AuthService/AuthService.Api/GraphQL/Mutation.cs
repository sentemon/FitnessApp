using System.Security.Claims;
using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.Logout;
using AuthService.Application.Commands.Register;
using AuthService.Application.Commands.ResetPassword;
using AuthService.Application.DTOs;
using AuthService.Infrastructure.Models;

namespace AuthService.Api.GraphQL;

public class Mutation
{
    private readonly IHttpContextAccessor _httpContextAccessor
        ;

    public Mutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<KeycloakTokenResponse> Register(RegisterDto input, [Service] RegisterCommandHandler registerCommandHandler)
    {
        var command = new RegisterCommand(input);
        var result = await registerCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<KeycloakTokenResponse> Login(LoginDto input, [Service] LoginCommandHandler loginCommandHandler)
    {
        var command = new LoginCommand(input);
        var result = await loginCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }
        
        return result.Response;
    }

    public async Task<string> Logout(string refreshToken, [Service] LogoutCommandHandler logoutCommandHandler)
    {
        var command = new LogoutCommand(refreshToken);
        var result = await logoutCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<string> ResetPassword(string newPassword, [Service] ResetPasswordCommandHandler resetPasswordCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new ResetPasswordCommand(userId, newPassword);
        var result = await resetPasswordCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.Error.Message);
        }

        return result.Response;
    }
}