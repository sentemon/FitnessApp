using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.DeleteWorkout;

public class DeleteWorkoutCommandHandler : ICommandHandler<DeleteWorkoutCommand, string>
{
    private readonly WorkoutDbContext _context;

    public DeleteWorkoutCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteWorkoutCommand command)
    {
        if (command.UserId is null)
        {
            return Result<string>.Failure(new Error("UserId is null."));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user is null)
        {
            return Result<string>.Failure(new Error("User not found."));
        }
        
        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == command.WorkoutId);
        if (workout is null)
        {
            return Result<string>.Failure(new Error("Workout not found."));
        }
        
        user.DeleteWorkout(workout);
        _context.Remove(workout);

        await _context.SaveChangesAsync();
        
        return Result<string>.Success("Workout was successfully deleted.");
    }
}