using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.DeleteSet;

public class DeleteSetCommandHandler : ICommandHandler<DeleteSetCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<DeleteSetCommandHandler> _logger;

    public DeleteSetCommandHandler(WorkoutDbContext context, ILogger<DeleteSetCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteSetCommand command)
    {
        var exercise = await _context.Exercises
            .Include(exercise => exercise.Sets)
            .FirstOrDefaultAsync(e => e.Id == command.ExerciseId);
        
        if (exercise is null)
        {
            _logger.LogWarning("Attempted to delete a set from an exercise that does not exist: ExerciseId: {ExerciseId}", command.ExerciseId);
            return Result<string>.Failure(new Error(ResponseMessages.ExerciseNotFound));
        }

        var set = exercise.Sets.FirstOrDefault(s => s.Id == command.Id);
        if (set is null)
        {
            _logger.LogWarning("Attempted to delete a set that does not exist: SetId: {SetId}", command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.SetNotFound));
        }
        
        exercise.DeleteSet(set);
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.SetDeleted);
    }
}