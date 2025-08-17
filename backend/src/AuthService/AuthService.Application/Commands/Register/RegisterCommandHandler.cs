using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using AuthService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO;
using Shared.DTO.Messages;

namespace AuthService.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, KeycloakTokenResponse>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(AuthDbContext context, IAuthService authService, IPublishEndpoint publishEndpoint, ILogger<RegisterCommandHandler> logger)
    {
        _context = context;
        _authService = authService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<IResult<KeycloakTokenResponse, Error>> HandleAsync(RegisterCommand command)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.Value == command.RegisterDto.Username || u.Email.Value == command.RegisterDto.Email);

        if (existingUser is not null)
        {
            if (existingUser.Username.Value == command.RegisterDto.Username)
            {
                _logger.LogWarning("Registration attempt with existing username: {Username}", command.RegisterDto.Username);
                return Result<KeycloakTokenResponse>.Failure(new Error("Username already exists."));
            }

            if (existingUser.Email.Value == command.RegisterDto.Email && existingUser.EmailVerified)
            {
                _logger.LogWarning("Registration attempt with existing email: {Email}", command.RegisterDto.Email);
                return Result<KeycloakTokenResponse>.Failure(new Error("Email already exists."));
            }
        }
        
        var user = await _authService.RegisterAsync(
            command.RegisterDto.FirstName,
            command.RegisterDto.LastName,
            command.RegisterDto.Username, 
            command.RegisterDto.Email,
            command.RegisterDto.Password
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User {Username} registered successfully.", user.Username.Value);

        await _publishEndpoint.Publish(new UserCreatedEventMessage(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username.Value,
            user.ImageUrl,
            user.CreatedAt
        ));
        
        _logger.LogInformation("UserCreatedEventMessage published for user {Username}.", user.Username.Value);

        try
        {
            var token = await _authService.LoginAsync(command.RegisterDto.Username, command.RegisterDto.Password);
            return Result<KeycloakTokenResponse>.Success(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging in after registration for user {Username}: {ErrorMessage}", command.RegisterDto.Username, ex.Message);
            return Result<KeycloakTokenResponse>.Failure(new Error(ex.Message));
        }
    }
}