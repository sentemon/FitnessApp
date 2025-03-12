using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Queries.GetAllWorkouts;

public class GetAllWorkoutsQueryHandler : IQueryHandler<GetAllWorkoutsQuery, List<WorkoutDto>>
{
    private readonly WorkoutDbContext _context;

    public GetAllWorkoutsQueryHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<WorkoutDto>, Error>> HandleAsync(GetAllWorkoutsQuery query)
    {
        var workouts = await _context.Workouts
            .Where(w => w.UserId == query.UserId || w.IsCustom)
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                    .ThenInclude(e => e.Sets)
            .Select(w => new WorkoutDto(
                w.Id,
                w.Title,
                w.Description,
                w.DurationInMinutes,
                w.Level,
                w.IsCustom,
                w.Url,
                w.ImageUrl,
                w.WorkoutExercises.Select(we => new ExerciseDto(
                    we.Exercise.Id,
                    we.Exercise.Name,
                    we.Exercise.Level,
                    we.Exercise.Sets.Select(s => new SetDto(
                        s.Id,
                        s.Reps,
                        s.Weight
                    )).ToArray()
                )).ToArray()
            ))
            .ToListAsync();

        return Result<List<WorkoutDto>>.Success(workouts);
    }
}