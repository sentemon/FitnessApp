using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Commands.SetImageUrl;

public class SetImageUrlCommandHandler : ICommandHandler<SetImageUrlCommand, string>
{
    private readonly AuthDbContext _context;
    private readonly ILogger<SetImageUrlCommandHandler> _logger;

    public SetImageUrlCommandHandler(AuthDbContext context, ILogger<SetImageUrlCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(SetImageUrlCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user is null)
        {
            _logger.LogWarning("Attempted to set image URL for a non-existing user: UserId: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        user.SetImageUrl(command.ImageUrl);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Image URL set for user: {UserId}", command.UserId);
        return Result<string>.Success(user.ImageUrl);
    }
}