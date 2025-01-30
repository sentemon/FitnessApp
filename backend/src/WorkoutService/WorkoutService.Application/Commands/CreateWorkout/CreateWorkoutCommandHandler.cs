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
        // ToDo: checks

        var workout = Workout.Create(
            command.WorkoutDto.Title,
            command.WorkoutDto.Description,
            command.WorkoutDto.Time,
            command.WorkoutDto.Level,
            command.UserId
        );

        foreach (var exerciseDto in command.WorkoutDto.Exercises)
        {
            var exercise = Exercise.Create(exerciseDto.Name, exerciseDto.Level, command.UserId);
            workout.AddExercise(exercise);
            
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
            workout.Time,
            workout.Level,
            null,
            command.WorkoutDto.Exercises
        );

        return Result<WorkoutDto>.Success(workoutDto);
    }
}