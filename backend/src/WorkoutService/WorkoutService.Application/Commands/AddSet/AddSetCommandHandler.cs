using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.AddSet;

public class AddSetCommandHandler : ICommandHandler<AddSetCommand, SetDto>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<AddSetCommandHandler> _logger;

    public AddSetCommandHandler(WorkoutDbContext context, ILogger<AddSetCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<SetDto, Error>> HandleAsync(AddSetCommand command)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == command.ExerciseId);
        if (exercise is null)
        {
            _logger.LogWarning("Attempted to add a set to an exercise that does not exist: ExerciseId: {ExerciseId}", command.ExerciseId);
            return Result<SetDto>.Failure(new Error(ResponseMessages.ExerciseNotFound));
        }

        var set = Set.Create(command.Reps, command.Weight, exercise.Id);
        exercise.AddSet(set);

        await _context.SaveChangesAsync();

        var setDto = new SetDto(set.Id, set.Reps, set.Weight);
            
        return Result<SetDto>.Success(setDto);
    }
}