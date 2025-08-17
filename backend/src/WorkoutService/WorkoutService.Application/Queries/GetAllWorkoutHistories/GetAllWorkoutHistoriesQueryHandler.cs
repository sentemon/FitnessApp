using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Queries.GetAllWorkoutHistories;

public class GetAllWorkoutHistoriesQueryHandler : IQueryHandler<GetAllWorkoutHistoriesQuery, List<WorkoutHistory>>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<GetAllWorkoutHistoriesQueryHandler> _logger;

    public GetAllWorkoutHistoriesQueryHandler(WorkoutDbContext context, ILogger<GetAllWorkoutHistoriesQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<List<WorkoutHistory>, Error>> HandleAsync(GetAllWorkoutHistoriesQuery query)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == query.UserId);
        
        if (user is null)
        {
            _logger.LogWarning("Attempted to retrieve workout histories for a user that does not exist: UserId: {UserId}", query.UserId);
            return Result<List<WorkoutHistory>>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var workoutHistories = _context.WorkoutHistories
            .AsNoTracking()
            .Include(wh => wh.Workout)
            .Include(wh => wh.ExerciseHistories)
                .ThenInclude(eh => eh.SetHistories)
            .Where(wh => wh.UserId == user.Id)
            .ToList();
        
        return Result<List<WorkoutHistory>>.Success(workoutHistories);
    }
}