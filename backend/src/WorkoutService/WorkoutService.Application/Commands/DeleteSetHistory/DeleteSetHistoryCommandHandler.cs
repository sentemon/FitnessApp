using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.DeleteSetHistory;

public class DeleteSetHistoryCommandHandler : ICommandHandler<DeleteSetHistoryCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<DeleteSetHistoryCommandHandler> _logger;

    public DeleteSetHistoryCommandHandler(WorkoutDbContext context, ILogger<DeleteSetHistoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteSetHistoryCommand command)
    {
        var exerciseHistory = await _context.ExerciseHistories
            .Include(eh => eh.SetHistories)
            .FirstOrDefaultAsync(eh => eh.Id == command.ExerciseHistoryId);
        
        if (exerciseHistory is null)
        {
            _logger.LogWarning("Attempted to delete a set history from an exercise history that does not exist: ExerciseHistoryId: {ExerciseHistoryId}", command.ExerciseHistoryId);
            return Result<string>.Failure(new Error(ResponseMessages.ExerciseHistoryNotFound));
        }

        var setHistory = exerciseHistory.SetHistories.FirstOrDefault(sh => sh.Id == command.Id);
        if (setHistory is null)
        {
            _logger.LogWarning("Attempted to delete a set history that does not exist: SetHistoryId: {SetHistoryId}", command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.SetHistoryNotFound));
        }
        
        exerciseHistory.DeleteSet(setHistory);
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetHistoryDeleted);
    }
}