using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Queries.ProfileSetUp;

public class ProfileSetUpQueryHandler : IQueryHandler<ProfileSetUpQuery, bool>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<ProfileSetUpQueryHandler> _logger;

    public ProfileSetUpQueryHandler(WorkoutDbContext context, ILogger<ProfileSetUpQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<bool, Error>> HandleAsync(ProfileSetUpQuery query)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == query.UserId);
        
        if (user is null)
        {
            _logger.LogWarning("Attempted to check profile setup for a user that does not exist: UserId: {UserId}", query.UserId);
            return Result<bool>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        return Result<bool>.Success(user.ProfileSetUp);
    }
}