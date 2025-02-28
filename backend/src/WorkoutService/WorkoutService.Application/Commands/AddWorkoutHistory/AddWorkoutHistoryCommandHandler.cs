using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.AddWorkoutHistory;

public class AddWorkoutHistoryCommandHandler : ICommandHandler<AddWorkoutHistoryCommand, WorkoutHistory>
{
    private readonly WorkoutDbContext _context;

    public AddWorkoutHistoryCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<WorkoutHistory, Error>> HandleAsync(AddWorkoutHistoryCommand command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user is null)
        {
            return Result<WorkoutHistory>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == command.WorkoutId);
        if (workout is null)
        {
            return Result<WorkoutHistory>.Failure(new Error(ResponseMessages.WorkoutNotFound));
        }

        var workoutHistory = WorkoutHistory.Create(command.DurationInMinutes, workout.Id, user.Id);
        _context.WorkoutHistories.Add(workoutHistory);

        await _context.SaveChangesAsync();

        return Result<WorkoutHistory>.Success(workoutHistory);
    }
}