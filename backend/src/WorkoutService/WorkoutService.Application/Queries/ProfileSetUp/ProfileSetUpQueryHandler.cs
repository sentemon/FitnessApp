using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Queries.ProfileSetUp;

public class ProfileSetUpQueryHandler : IQueryHandler<ProfileSetUpQuery, bool>
{
    private readonly WorkoutDbContext _context;

    public ProfileSetUpQueryHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<bool, Error>> HandleAsync(ProfileSetUpQuery query)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.UserId);
        if (user is null)
        {
            return Result<bool>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        return Result<bool>.Success(user.ProfileSetUp);
    }
}