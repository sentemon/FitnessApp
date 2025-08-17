using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.AddWorkoutHistory;

public class AddWorkoutHistoryCommandHandler : ICommandHandler<AddWorkoutHistoryCommand, WorkoutHistory>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<AddWorkoutHistoryCommandHandler> _logger;

    public AddWorkoutHistoryCommandHandler(WorkoutDbContext context, ILogger<AddWorkoutHistoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<WorkoutHistory, Error>> HandleAsync(AddWorkoutHistoryCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user is null)
        {
            _logger.LogWarning("Attempted to add a workout history for a user that does not exist: UserId: {UserId}", command.UserId);
            return Result<WorkoutHistory>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == command.WorkoutId);
        if (workout is null)
        {
            _logger.LogWarning("Attempted to add a workout history for a workout that does not exist: WorkoutId: {WorkoutId}", command.WorkoutId);
            return Result<WorkoutHistory>.Failure(new Error(ResponseMessages.WorkoutNotFound));
        }

        var workoutHistory = WorkoutHistory.Create(workout.Id, user.Id);
        _context.WorkoutHistories.Add(workoutHistory);

        await _context.SaveChangesAsync();

        return Result<WorkoutHistory>.Success(workoutHistory);
    }
}