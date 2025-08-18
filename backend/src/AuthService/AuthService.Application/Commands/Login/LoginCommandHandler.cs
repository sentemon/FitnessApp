using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, KeycloakTokenResponse>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IAuthService authService, AuthDbContext context, ILogger<LoginCommandHandler> logger)
    {
        _authService = authService;
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<KeycloakTokenResponse, Error>> HandleAsync(LoginCommand command)
    {
        var isEmail = IsEmail(command.LoginDto.Username);
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => (isEmail ? u.Email.Value : u.Username.Value) == command.LoginDto.Username);

        if (existingUser is null && isEmail)
        {
            _logger.LogWarning("Login attempt with non-existing email: {Email}", command.LoginDto.Username);
            return Result<KeycloakTokenResponse>.Failure(new Error("User with this email does not exist."));
        }
        
        if (existingUser is null && !isEmail)
        {
            _logger.LogWarning("Login attempt with non-existing username: {Username}", command.LoginDto.Username);
            return Result<KeycloakTokenResponse>.Failure(new Error("User with this username does not exist."));
        }
        
        if (existingUser is not null && isEmail && !existingUser.EmailVerified)
        {
            _logger.LogWarning("Login attempt with unverified email: {Email}", command.LoginDto.Username);
            return Result<KeycloakTokenResponse>.Failure(new Error("Email is not verified. Try to login with the username."));
        }
        
        try
        {
            var token = await _authService.LoginAsync(command.LoginDto.Username, command.LoginDto.Password);
            _logger.LogWarning("User {Username} logged in successfully.", command.LoginDto.Username);
            return Result<KeycloakTokenResponse>.Success(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during login for user {Username}: {ErrorMessage}", command.LoginDto.Username, e.Message);
            return Result<KeycloakTokenResponse>.Failure(new Error("Wrong password."));
        }
    }

    private static bool IsEmail(string value)
    {
        return value.Contains('@') && value.Contains('.');
    }
}