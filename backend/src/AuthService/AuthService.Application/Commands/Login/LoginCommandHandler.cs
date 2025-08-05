using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, KeycloakTokenResponse>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService, AuthDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    public async Task<IResult<KeycloakTokenResponse, Error>> HandleAsync(LoginCommand command)
    {
        var isEmail = IsEmail(command.LoginDto.Username);
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => (isEmail ? u.Email.Value : u.Username.Value) == command.LoginDto.Username);

        if (existingUser is null && isEmail)
        {
            return Result<KeycloakTokenResponse>.Failure(new Error("User with this email does not exist."));
        }
        
        if (existingUser is null && !isEmail)
        {
            return Result<KeycloakTokenResponse>.Failure(new Error("User with this username does not exist."));
        }
        
        if (existingUser is not null && !existingUser.EmailVerified)
        {
            return Result<KeycloakTokenResponse>.Failure(new Error("Email is not verified. Try to login with the username."));
        }
        
        try
        {
            var token = await _authService.LoginAsync(command.LoginDto.Username, command.LoginDto.Password);
            return Result<KeycloakTokenResponse>.Success(token);
        }
        catch (Exception)
        {
            // Log the exception
            return Result<KeycloakTokenResponse>.Failure(new Error("Wrong password."));
        }
    }

    private static bool IsEmail(string value)
    {
        return value.Contains('@') && value.Contains('.');
    }
}