using AuthService.Application.Commands.VerifyEmail;
using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.SendVerifyEmail;

public class SendVerifyEmailCommandHandler : ICommandHandler<SendVerifyEmailCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly IAuthService _authService;
    private readonly ILogger<SendVerifyEmailCommandHandler> _logger;

    public SendVerifyEmailCommandHandler(AuthDbContext context, IAuthService authService, ILogger<SendVerifyEmailCommandHandler> logger)
    {
        _context = context;
        _authService = authService;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(SendVerifyEmailCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user == null)
        {
            _logger.LogWarning("Send verify email attempt with non-existing user ID: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var result = await _authService.SendVerifyEmailAsync(user.Id);

        if (!result)
        {
            _logger.LogError("Error during sending verify email for UserId: {UserId}", user.Id);
            return Result<string>.Failure(new Error(ResponseMessages.ErrorDuringSendVerifyEmail));
        }
        
        _logger.LogInformation("Verify email sent successfully for UserId: {UserId}", user.Id);
        return Result<string>.Success(ResponseMessages.EmailVerificationSentSuccessfully);
    }
}