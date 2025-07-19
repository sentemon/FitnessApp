using System.Security.Claims;
using AuthService.Application.Commands.Follow;
using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.Logout;
using AuthService.Application.Commands.Register;
using AuthService.Application.Commands.ResetPassword;
using AuthService.Application.Commands.SendVerifyEmail;
using AuthService.Application.Commands.Unfollow;
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

    public async Task<KeycloakTokenResponse> Register(RegisterDto input, [Service] RegisterCommandHandler registerCommandHandler)
    {
        var command = new RegisterCommand(input);
        var result = await registerCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }
        
        SetCookie("token", result.Response.AccessToken, result.Response.ExpiresIn);
        SetCookie("refreshToken", result.Response.RefreshToken, result.Response.ExpiresIn);

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
        
        SetCookie("token", result.Response.AccessToken, result.Response.ExpiresIn);
        SetCookie("refreshToken", result.Response.RefreshToken, result.Response.ExpiresIn);
        
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
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token");
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");

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

    public async Task<string> Follow(string targetUserId, [Service] FollowCommandHandler followCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new FollowCommand(targetUserId, userId);
        var result = await followCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> Unfollow(string targetUserId, [Service] UnfollowCommandHandler unfollowCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new UnfollowCommand(targetUserId, userId);
        var result = await unfollowCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    private void SetCookie(string name, string value, long expiresInSeconds)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(name, value, new CookieOptions
        {
            Domain = ".sentemon.me",
            Path = "/",
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.None,
            MaxAge = TimeSpan.FromSeconds(expiresInSeconds)
        });
    }
}