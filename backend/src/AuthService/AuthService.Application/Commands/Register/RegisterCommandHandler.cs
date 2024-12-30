using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using AuthService.Persistence;
using MassTransit;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO;

namespace AuthService.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, KeycloakTokenResponse>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;
    private readonly IPublishEndpoint _publishEndpoint;

    public RegisterCommandHandler(AuthDbContext context, IAuthService authService, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _authService = authService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IResult<KeycloakTokenResponse, Error>> HandleAsync(RegisterCommand command)
    {
        var user = await _authService.RegisterAsync(
            command.RegisterDto.FirstName,
            command.RegisterDto.LastName,
            command.RegisterDto.Username, 
            command.RegisterDto.Email,
            command.RegisterDto.Password
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new UserCreatedEvent(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username.Value,
            user.ImageUrl,
            user.CreatedAt
        ));

        var token = await _authService.LoginAsync(command.RegisterDto.Username, command.RegisterDto.Password);
        
        return Result<KeycloakTokenResponse>.Success(token);
    }
}