using AuthService.Application.Commands.VerifyEmail;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.SendVerifyEmail;

public class SendVerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;

    public SendVerifyEmailCommandHandler(AuthDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<IResult<string, Error>> HandleAsync(VerifyEmailCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user == null)
        {
            return Result<string>.Failure(new Error("User not found."));
        }
        
        var result = await _authService.SendVerifyEmailAsync(command.UserId);

        if (!result)
        {
            return Result<string>.Failure(new Error("Something was wrong."));
        }
        
        return Result<string>.Success("Email verification sent successfully.");
    }
}