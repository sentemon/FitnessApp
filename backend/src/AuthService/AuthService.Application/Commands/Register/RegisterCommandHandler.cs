using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly IKeycloakService _keycloakService;

    public RegisterCommandHandler(AuthDbContext context, IKeycloakService keycloakService)
    {
        _context = context;
        _keycloakService = keycloakService;
    }

    public async Task<IResult<string, Error>> HandleAsync(RegisterCommand command)
    {
        var user = await _keycloakService.RegisterAsync(
            command.RegisterDto.FirstName,
            command.RegisterDto.LastName,
            command.RegisterDto.Username, 
            command.RegisterDto.Email,
            command.RegisterDto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = await _keycloakService.LoginAsync(command.RegisterDto.Username, command.RegisterDto.Password);
        
        return Result<string>.Success(token.AccessToken);
    }
}