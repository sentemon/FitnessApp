using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Queries.GetWorkoutByUrl;

public class GetWorkoutByUrlQueryHandler : IQueryHandler<GetWorkoutByUrlQuery, WorkoutDto>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<GetWorkoutByUrlQueryHandler> _logger;

    public GetWorkoutByUrlQueryHandler(WorkoutDbContext context, ILogger<GetWorkoutByUrlQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<WorkoutDto, Error>> HandleAsync(GetWorkoutByUrlQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.Url))
        {
            _logger.LogWarning("Attempted to get a workout by an empty URL.");
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
            _logger.LogWarning("Attempted to get a workout that does not exist: Url: {Url}", query.Url);
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