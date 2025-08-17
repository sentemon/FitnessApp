using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.CompleteWorkoutHistory;

public class CompleteWorkoutHistoryCommandHandler : ICommandHandler<CompleteWorkoutHistoryCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<CompleteWorkoutHistoryCommandHandler> _logger;

    public CompleteWorkoutHistoryCommandHandler(WorkoutDbContext context, ILogger<CompleteWorkoutHistoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(CompleteWorkoutHistoryCommand command)
    {
        var workoutHistory = await _context.WorkoutHistories.FirstOrDefaultAsync(wh => wh.Id == command.Id);
        if (workoutHistory is null)
        {
            _logger.LogWarning("Attempted to complete a workout history that does not exist: WorkoutHistoryId: {WorkoutHistoryId}", command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.WorkoutHistoryNotFound));
        }
        
        workoutHistory.Complete(command.DurationInMinutes);

        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.WorkoutHistoryCompleted);
    }
}