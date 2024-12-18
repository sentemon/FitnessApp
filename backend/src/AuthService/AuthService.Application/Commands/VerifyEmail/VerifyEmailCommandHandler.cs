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

    public VerifyEmailCommandHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(VerifyEmailCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        user.VerifyEmail();
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.EmailVerifiedSuccessfully);
    }
}