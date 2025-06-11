using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.CompleteWorkoutHistory;

public class CompleteWorkoutHistoryCommandHandler : ICommandHandler<CompleteWorkoutHistoryCommand, string>
{
    private readonly WorkoutDbContext _context;

    public CompleteWorkoutHistoryCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(CompleteWorkoutHistoryCommand command)
    {
        var workoutHistory = await _context.WorkoutHistories.FirstOrDefaultAsync(wh => wh.Id == command.Id);
        if (workoutHistory is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.WorkoutHistoryNotFound));
        }
        
        workoutHistory.Complete(command.DurationInMinutes);

        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.WorkoutHistoryCompleted);
    }
}