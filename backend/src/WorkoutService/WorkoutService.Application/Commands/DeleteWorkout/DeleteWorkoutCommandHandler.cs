using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.DeleteWorkout;

public class DeleteWorkoutCommandHandler : ICommandHandler<DeleteWorkoutCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<DeleteWorkoutCommandHandler> _logger;

    public DeleteWorkoutCommandHandler(WorkoutDbContext context, ILogger<DeleteWorkoutCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteWorkoutCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user is null)
        {
            _logger.LogWarning("Attempted to delete a workout for a user that does not exist: UserId: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == command.WorkoutId);
        if (workout is null)
        {
            _logger.LogWarning("Attempted to delete a workout that does not exist: WorkoutId: {WorkoutId}", command.WorkoutId);
            return Result<string>.Failure(new Error(ResponseMessages.WorkoutNotFound));
        }
        
        user.DeleteWorkout(workout);
        _context.Remove(workout);

        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.WorkoutDeleted);
    }
}