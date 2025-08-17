using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.Application.Extensions;
using WorkoutService.Application.DTOs;
using WorkoutService.Application.Validators;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler : ICommandHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<CreateWorkoutCommandHandler> _logger;

    private readonly IValidator<CreateWorkoutDto> _validator;

    public CreateWorkoutCommandHandler(WorkoutDbContext context, ILogger<CreateWorkoutCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        
        _validator = new CreateWorkoutValidator();
    }

    public async Task<IResult<WorkoutDto, Error>> HandleAsync(CreateWorkoutCommand command)
    {
        var errors = await _validator.ValidateResultAsync(command.CreateWorkoutDto);
        if (errors is not null)
        {
            _logger.LogWarning("Validation failed for CreateWorkoutCommand: {Errors}", errors);
            return Result<WorkoutDto>.Failure(new Error(errors));
        }

        var user = await _context.Users
            .Include(u => u.Workouts)
            .Include(u => u.Exercises)
            .FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user is null)
        {
            _logger.LogWarning("Attempted to create a workout for a user that does not exist: UserId: {UserId}", command.UserId);
            return Result<WorkoutDto>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var workout = Workout.Create(
            command.CreateWorkoutDto.Title,
            command.CreateWorkoutDto.Description,
            (uint)command.CreateWorkoutDto.DurationInMinutes,
            command.CreateWorkoutDto.Level,
            user.Id
        );

        var exercises = new List<Exercise>();

        foreach (var exerciseDto in command.CreateWorkoutDto.Exercises)
        {
            if (string.IsNullOrWhiteSpace(exerciseDto.Name))
            {
                _logger.LogWarning("Attempted to create an exercise with an empty name in CreateWorkoutCommand");
                return Result<WorkoutDto>.Failure(new Error("Name of exercise cannot be empty"));
            }

            var exercise = Exercise.Create(exerciseDto.Name, exerciseDto.Level, user.Id);
            exercises.Add(exercise);
        }

        await _context.Exercises.AddRangeAsync(exercises);
        await _context.SaveChangesAsync();

        foreach (var exercise in exercises)
        {
            workout.AddExercise(exercise);

            foreach (var setDto in command.CreateWorkoutDto.Exercises.First(e => e.Name == exercise.Name).Sets)
            {
                var set = Set.Create(setDto.Reps, setDto.Weight, exercise.Id);
                exercise.AddSet(set);
            }
        }
        
        // ToDo: get from File Service
        workout.SetImageUrl("https://example.com/image");

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        var workoutDto = new WorkoutDto(
            workout.Id,
            workout.Title,
            workout.Description,
            workout.DurationInMinutes,
            workout.Level,
            workout.IsCustom,
            workout.Url,
            workout.ImageUrl,
            exercises.Select(e => new ExerciseDto(
                e.Id,
                e.Name,
                e.Level,
                e.Sets.ToList().Select(s => new SetDto(s.Id, s.Reps, s.Weight)).ToArray())
            ).ToArray()
        );

        return Result<WorkoutDto>.Success(workoutDto);
    }
}