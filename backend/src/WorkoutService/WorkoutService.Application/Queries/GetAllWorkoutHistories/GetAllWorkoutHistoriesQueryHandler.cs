using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Queries.GetAllWorkoutHistories;

public class GetAllWorkoutHistoriesQueryHandler : IQueryHandler<GetAllWorkoutHistoriesQuery, List<WorkoutHistory>>
{
    private readonly WorkoutDbContext _context;

    public GetAllWorkoutHistoriesQueryHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<WorkoutHistory>, Error>> HandleAsync(GetAllWorkoutHistoriesQuery query)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == query.UserId);
        
        if (user is null)
        {
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