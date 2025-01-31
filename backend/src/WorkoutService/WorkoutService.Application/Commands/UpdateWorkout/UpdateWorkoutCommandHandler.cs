using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.UpdateWorkout;

public class UpdateWorkoutCommandHandler : ICommandHandler<UpdateWorkoutCommand, string>
{
    private readonly WorkoutDbContext _context;

    public UpdateWorkoutCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(UpdateWorkoutCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.UpdateWorkoutDto.Title) || command.UpdateWorkoutDto.Title.Length > 100)
        {
            return Result<string>.Failure(new Error("Title of workout cannot be empty or longer than 100 characters."));
        }
        
        if (string.IsNullOrWhiteSpace(command.UpdateWorkoutDto.Description) || command.UpdateWorkoutDto.Description.Length > 500)
        {
            return Result<string>.Failure(new Error("Description of workout cannot be empty or longer than 500 characters."));
        }

        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == Guid.Parse(command.UpdateWorkoutDto.Id));

        if (workout is null)
        {
            return Result<string>.Failure(new Error("Workout not found."));
        }
        
        workout.Update(
            command.UpdateWorkoutDto.Title,
            command.UpdateWorkoutDto.Description,
            command.UpdateWorkoutDto.DurationInMinutes,
            command.UpdateWorkoutDto.Level
        );

        await _context.SaveChangesAsync();
        
        return Result<string>.Success("Workout is successfully updated");
    }
}