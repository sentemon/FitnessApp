using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.Logout;
using AuthService.Application.Commands.Register;
using AuthService.Application.DTOs;

namespace AuthService.Api.GraphQL;

public class Mutation
{
    public async Task<string> Register(RegisterDto input, [Service] RegisterCommandHandler registerCommandHandler)
    {
        var command = new RegisterCommand(input);
        var result = await registerCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }
        
        // ToDo: return Keycloak Token Response 
        return result.Response;
    }
    
    public async Task<string> Login(LoginDto input, [Service] LoginCommandHandler loginCommandHandler)
    {
        var command = new LoginCommand(input);
        var result = await loginCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }
        
        // ToDo: return Keycloak Token Response 
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
}