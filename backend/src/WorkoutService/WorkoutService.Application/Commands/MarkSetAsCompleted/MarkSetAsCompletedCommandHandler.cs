using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.MarkSetAsCompleted;

public class MarkSetAsCompletedCommandHandler : ICommandHandler<MarkSetAsCompletedCommand, string>
{
    private readonly WorkoutDbContext _context;

    public MarkSetAsCompletedCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(MarkSetAsCompletedCommand command)
    {
        var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == command.Id && s.ExerciseId == command.ExerciseId);

        if (set is null)
        {
            return Result<string>.Failure(new Error("Set not found."));
        }
        
        set.MarkAsCompleted();
        await _context.SaveChangesAsync();
        
        return Result<string>.Success("Set marked as completed");
    }
}