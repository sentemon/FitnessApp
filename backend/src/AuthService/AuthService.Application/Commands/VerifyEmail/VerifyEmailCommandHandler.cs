using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;

    public VerifyEmailCommandHandler(AuthDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<IResult<string, Error>> HandleAsync(VerifyEmailCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var result = await _authService.VerifyEmailAsync(user.Id);
        
        if (!result)
        {
            return Result<string>.Failure(new Error(ResponseMessages.ErrorDuringVerifyEmail));
        }
        
        user.VerifyEmail();
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.EmailVerifiedSuccessfully);
    }
}