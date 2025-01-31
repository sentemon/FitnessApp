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

    private readonly IValidator<WorkoutDto> _validator;

    public CreateWorkoutCommandHandler(WorkoutDbContext context)
    {
        _context = context;
        _validator = new CreateWorkoutValidator();
    }

    public async Task<IResult<WorkoutDto, Error>> HandleAsync(CreateWorkoutCommand command)
    {
        var errors = await _validator.ValidateAsync(command.WorkoutDto);

        if (!errors.IsValid)
        {
            return Result<WorkoutDto>.Failure(new Error(errors.Errors.ToString() ?? "Validation failed."));
        }

        if (command.UserId is null)
        {
            return Result<WorkoutDto>.Failure(new Error("UserId cannot be null."));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user is null)
        {
            return Result<WorkoutDto>.Failure(new Error("User not found."));
        }
        
        var workout = Workout.Create(
            command.WorkoutDto.Title,
            command.WorkoutDto.Description,
            command.WorkoutDto.DurationInMinutes,
            command.WorkoutDto.Level,
            command.UserId
        );

        foreach (var exerciseDto in command.WorkoutDto.Exercises)
        {
            if (string.IsNullOrWhiteSpace(exerciseDto.Name))
            {
                return Result<WorkoutDto>.Failure(new Error("Name of exercise cannot be empty"));
            }
            
            var exercise = Exercise.Create(exerciseDto.Name, exerciseDto.Level, command.UserId);
            workout.AddExercise(exercise);
            user.AddExercise(exercise);
            
            await _context.SaveChangesAsync();
            
            foreach (var setDto in exerciseDto.Sets)
            {
                var set = Set.Create(setDto.Reps, setDto.Weight, exercise.Id);
                exercise.AddSet(set);
            }
        }
        
        user.AddWorkout(workout);
        
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();
        
        var workoutDto = new WorkoutDto(
            workout.Title,
            workout.Description,
            workout.DurationInMinutes,
            workout.Level,
            null,
            command.WorkoutDto.Exercises
        );

        return Result<WorkoutDto>.Success(workoutDto);
    }
}