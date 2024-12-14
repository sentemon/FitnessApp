using AuthService.Application.DTOs;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, UserDto>
{
    private readonly AuthDbContext _context;
    private readonly IKeycloakService _keycloakService;

    public RegisterCommandHandler(AuthDbContext context, IKeycloakService keycloakService)
    {
        _context = context;
        _keycloakService = keycloakService;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(RegisterCommand command)
    {
        var user = await _keycloakService.RegisterAsync(
            command.RegisterDto.FirstName,
            command.RegisterDto.LastName,
            command.RegisterDto.Username, 
            command.RegisterDto.Email, 
            command.RegisterDto.Password);

        if (user == null)
        {
            return Result<UserDto>.Failure(new Error("User cannot be null."));
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto(
            user.FirstName,
            user.LastName,
            user.Username,
            user.Email,
            user.ImageUrl);
        
        return Result<UserDto>.Success(userDto);
    }
}