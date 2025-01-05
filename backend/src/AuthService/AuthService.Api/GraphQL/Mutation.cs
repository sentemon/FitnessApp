using System.Security.Claims;
using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.Logout;
using AuthService.Application.Commands.Register;
using AuthService.Application.Commands.ResetPassword;
using AuthService.Application.Commands.SendVerifyEmail;
using AuthService.Application.Commands.UpdateUser;
using AuthService.Application.Commands.VerifyEmail;
using AuthService.Application.DTOs;
using AuthService.Infrastructure.Models;

namespace AuthService.Api.GraphQL;

public class Mutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Mutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    
    /* ToDo: When all the services are running in the docker that's it
       System.Net.Http.HttpClient.KeycloakClient.LogicalHandler[100]
       2025-01-05 22:30:39       Start processing HTTP request POST http://keycloak:8080/realms/fitness-app-realm/protocol/openid-connect/token
       2025-01-05 22:30:39 info: System.Net.Http.HttpClient.KeycloakClient.ClientHandler[100]
       2025-01-05 22:30:39       Sending HTTP request POST http://keycloak:8080/realms/fitness-app-realm/protocol/openid-connect/token
       2025-01-05 22:30:39 info: System.Net.Http.HttpClient.KeycloakClient.ClientHandler[101]
       2025-01-05 22:30:39       Received HTTP response headers after 91.8503ms - 404
       2025-01-05 22:30:39 info: System.Net.Http.HttpClient.KeycloakClient.LogicalHandler[101]
       2025-01-05 22:30:39       End processing HTTP request after 100.8284ms - 404
     */
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

    public async Task<string> SendVerifyEmail([Service] SendVerifyEmailCommandHandler sendVerifyEmailCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new SendVerifyEmailCommand(userId);
        var result = await sendVerifyEmailCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<string> VerifyEmail([Service] VerifyEmailCommandHandler verifyEmailCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new VerifyEmailCommand(userId);
        var result = await verifyEmailCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<string> UpdateUser(UpdateUserDto input, [Service] UpdateUserCommandHandler updateUserCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new UpdateUserCommand(input, userId);
        var result = await updateUserCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}