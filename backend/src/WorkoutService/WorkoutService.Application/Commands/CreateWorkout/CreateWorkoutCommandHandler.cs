using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;
using WorkoutService.Application.Validators;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler : ICommandHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly WorkoutDbContext _context;

    private readonly IValidator<CreateWorkoutDto> _validator;

    public CreateWorkoutCommandHandler(WorkoutDbContext context)
    {
        _context = context;
        _validator = new CreateWorkoutValidator();
    }

    public async Task<IResult<WorkoutDto, Error>> HandleAsync(CreateWorkoutCommand command)
    {
        var errors = await _validator.ValidateAsync(command.CreateWorkoutDto);
        if (!errors.IsValid)
        {
            return Result<WorkoutDto>.Failure(new Error(errors.Errors.ToString() ?? "Validation failed."));
        }

        if (command.UserId is null)
        {
            return Result<WorkoutDto>.Failure(new Error("UserId cannot be null."));
        }

        var user = await _context.Users
            .Include(u => u.Workouts)
            .Include(u => u.Exercises)
            .FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user is null)
        {
            return Result<WorkoutDto>.Failure(new Error("User not found."));
        }

        var workout = Workout.Create(
            command.CreateWorkoutDto.Title,
            command.CreateWorkoutDto.Description,
            command.CreateWorkoutDto.DurationInMinutes,
            command.CreateWorkoutDto.Level,
            command.UserId
        );

        var exercises = new List<Exercise>();

        foreach (var exerciseDto in command.CreateWorkoutDto.Exercises)
        {
            if (string.IsNullOrWhiteSpace(exerciseDto.Name))
            {
                return Result<WorkoutDto>.Failure(new Error("Name of exercise cannot be empty"));
            }

            var exercise = Exercise.Create(exerciseDto.Name, exerciseDto.Level, command.UserId);
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
            workout.Title,
            workout.Description,
            workout.DurationInMinutes,
            workout.Level,
            workout.ImageUrl,
            command.CreateWorkoutDto.Exercises
        );

        return Result<WorkoutDto>.Success(workoutDto);
    }
}