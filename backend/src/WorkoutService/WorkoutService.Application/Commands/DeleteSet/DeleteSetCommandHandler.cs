using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.DeleteSet;

public class DeleteSetCommandHandler : ICommandHandler<DeleteSetCommand, string>
{
    private readonly WorkoutDbContext _context;

    public DeleteSetCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteSetCommand command)
    {
        var exercise = await _context.Exercises
            .Include(exercise => exercise.Sets)
            .FirstOrDefaultAsync(e => e.Id == command.ExerciseId);
        
        if (exercise is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.ExerciseNotFound));
        }

        var set = exercise.Sets.FirstOrDefault(s => s.Id == command.Id);
        if (set is null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.SetNotFound));
        }
        
        exercise.DeleteSet(set);
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetDeleted);
    }
}