using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(AuthDbContext context, IAuthService authService, ILogger<VerifyEmailCommandHandler> logger)
    {
        _context = context;
        _authService = authService;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(VerifyEmailCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user == null)
        {
            _logger.LogWarning("Verify email attempt with non-existing user ID: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var result = await _authService.VerifyEmailAsync(user.Id);
        
        if (!result)
        {
            _logger.LogError("Error during verifying email for UserId: {UserId}", user.Id);
            return Result<string>.Failure(new Error(ResponseMessages.ErrorDuringVerifyEmail));
        }
        
        user.VerifyEmail();
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Email verified successfully for UserId: {UserId}", user.Id);
        return Result<string>.Success(ResponseMessages.EmailVerifiedSuccessfully);
    }
}