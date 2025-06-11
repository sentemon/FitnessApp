using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.DeleteSetHistory;

public class DeleteSetHistoryCommandHandler : ICommandHandler<DeleteSetHistoryCommand, string>
{
    private readonly WorkoutDbContext _context;

    public DeleteSetHistoryCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteSetHistoryCommand command)
    {
        var exerciseHistory = await _context.ExerciseHistories
            .Include(eh => eh.SetHistories)
            .FirstOrDefaultAsync(eh => eh.Id == command.ExerciseHistoryId);
        
        if (exerciseHistory is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.ExerciseHistoryNotFound));
        }

        var setHistory = exerciseHistory.SetHistories.FirstOrDefault(sh => sh.Id == command.Id);
        if (setHistory is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.SetHistoryNotFound));
        }
        
        exerciseHistory.DeleteSet(setHistory);
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetHistoryDeleted);
    }
}