using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Models;
using AuthService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, KeycloakTokenResponse>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;

    public RegisterCommandHandler(AuthDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
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

        var token = await _authService.LoginAsync(command.RegisterDto.Username, command.RegisterDto.Password);
        
        return Result<KeycloakTokenResponse>.Success(token);
    }
}