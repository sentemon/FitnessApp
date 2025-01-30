using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler : ICommandHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly WorkoutDbContext _dbContext;

    public CreateWorkoutCommandHandler(WorkoutDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IResult<WorkoutDto, Error>> HandleAsync(CreateWorkoutCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.WorkoutDto.Title) || command.WorkoutDto.Title.Length > 100)
        {
            return Result<WorkoutDto>.Failure(new Error("Title of workout cannot be empty or longer than 100 characters."));
        }
        
        if (string.IsNullOrWhiteSpace(command.WorkoutDto.Description) || command.WorkoutDto.Description.Length > 500)
        {
            return Result<WorkoutDto>.Failure(new Error("Description of workout cannot be empty or longer than 500 characters."));
        }

        if (command.UserId is null)
        {
            return Result<WorkoutDto>.Failure(new Error("UserId cannot be null."));
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
            
            await _dbContext.SaveChangesAsync();
            
            foreach (var setDto in exerciseDto.Sets)
            {
                var set = Set.Create(setDto.Reps, setDto.Weight, exercise.Id);
                exercise.AddSet(set);
            }
        }

        _dbContext.Workouts.Add(workout);
        await _dbContext.SaveChangesAsync();
        
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