using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.MarkSetHistoryAsCompleted;

public class MarkSetHistoryAsCompletedCommandHandler : ICommandHandler<MarkSetHistoryAsCompletedCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<MarkSetHistoryAsCompletedCommandHandler> _logger;

    public MarkSetHistoryAsCompletedCommandHandler(WorkoutDbContext context, ILogger<MarkSetHistoryAsCompletedCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(MarkSetHistoryAsCompletedCommand command)
    {
        var setHistory = await _context.SetHistories.FirstOrDefaultAsync(sh => sh.Id == command.Id);

        if (setHistory is null)
        {
            _logger.LogWarning("Attempted to mark a set history as completed that does not exist: SetHistoryId: {SetHistoryId}", command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.SetHistoryNotFound));
        }
        
        setHistory.MarkAsCompleted();
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetCompleted);
    }
}