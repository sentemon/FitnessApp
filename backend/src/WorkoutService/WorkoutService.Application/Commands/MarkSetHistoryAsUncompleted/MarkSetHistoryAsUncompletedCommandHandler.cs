using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.MarkSetHistoryAsUncompleted;

public class MarkSetHistoryAsUncompletedCommandHandler : ICommandHandler<MarkSetHistoryAsUncompletedCommand, string>
{
    private readonly WorkoutDbContext _context;

    public MarkSetHistoryAsUncompletedCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(MarkSetHistoryAsUncompletedCommand command)
    {
        var setHistory = await _context.SetHistories.FirstOrDefaultAsync(sh => sh.Id == command.Id && sh.ExerciseHistoryId == command.ExerciseHistoryId);

        if (setHistory is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.SetHistoryNotFound));
        }
        
        setHistory.MarkAsUncompleted();
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetUncompleted);
    }
}