using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.Commands.MarkSetHistoryAsCompleted;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.MarkSetHistoryAsUncompleted;

public class MarkSetHistoryAsUncompletedCommandHandler : ICommandHandler<MarkSetHistoryAsUncompletedCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<MarkSetHistoryAsCompletedCommandHandler> _logger;

    public MarkSetHistoryAsUncompletedCommandHandler(WorkoutDbContext context, ILogger<MarkSetHistoryAsCompletedCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(MarkSetHistoryAsUncompletedCommand command)
    {
        var setHistory = await _context.SetHistories.FirstOrDefaultAsync(sh => sh.Id == command.Id);

        if (setHistory is null)
        {
            _logger.LogWarning("Attempted to mark a set history as uncompleted that does not exist: SetHistoryId: {SetHistoryId}", command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.SetHistoryNotFound));
        }
        
        setHistory.MarkAsUncompleted();
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetUncompleted);
    }
}