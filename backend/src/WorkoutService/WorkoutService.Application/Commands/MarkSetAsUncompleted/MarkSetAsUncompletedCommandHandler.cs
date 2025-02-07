using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.MarkSetAsUncompleted;

public class MarkSetAsUncompletedCommandHandler : ICommandHandler<MarkSetAsUncompletedCommand, string>
{
    private readonly WorkoutDbContext _context;

    public MarkSetAsUncompletedCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(MarkSetAsUncompletedCommand command)
    {
        var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == command.Id && s.ExerciseId == command.ExerciseId);

        if (set is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.SetNotFound));
        }
        
        // set.MarkAsUncompleted();
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetUncompleted);
    }
}