using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Queries.GetWorkoutByUrl;

public class GetWorkoutByUrlQueryHandler : IQueryHandler<GetWorkoutByUrlQuery, WorkoutDto>
{
    private readonly WorkoutDbContext _context;

    public GetWorkoutByUrlQueryHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<WorkoutDto, Error>> HandleAsync(GetWorkoutByUrlQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.Url))
        {
            return Result<WorkoutDto>.Failure(new Error(ResponseMessages.WorkoutUrlEmpty));
        }

        var workout = await _context.Workouts
            .AsNoTracking()
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                    .ThenInclude(e => e.Sets)
            .FirstOrDefaultAsync(w => w.Url == query.Url);

        if (workout is null)
        {
            return Result<WorkoutDto>.Failure(new Error(ResponseMessages.WorkoutNotFound));
        }

        var workoutDto = new WorkoutDto(
            workout.Id,
            workout.Title,
            workout.Description,
            workout.DurationInMinutes,
            workout.Level,
            workout.IsCustom,
            workout.Url,
            workout.ImageUrl,
            workout.WorkoutExercises.Select(we => new ExerciseDto(
                we.Exercise.Id,
                we.Exercise.Name,
                we.Exercise.Level,
                we.Exercise.Sets.Select(s => new SetDto(
                    s.Id,
                    s.Reps,
                    s.Weight
                )).ToArray()
            )).ToArray()
        );
        
        return Result<WorkoutDto>.Success(workoutDto);
    }
}