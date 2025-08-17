using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Constants;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.AddSetHistory;

public class AddSetHistoryCommandHandler : ICommandHandler<AddSetHistoryCommand, SetDto>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<AddSetHistoryCommandHandler> _logger;

    public AddSetHistoryCommandHandler(WorkoutDbContext context, ILogger<AddSetHistoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<SetDto, Error>> HandleAsync(AddSetHistoryCommand command)
    {
        var exerciseHistory = await _context.ExerciseHistories.FirstOrDefaultAsync(e => e.Id == command.ExerciseHistoryId);
        if (exerciseHistory is null)
        {
            _logger.LogWarning("Attempted to add a set history to an exercise history that does not exist: ExerciseHistoryId: {ExerciseHistoryId}", command.ExerciseHistoryId);
            return Result<SetDto>.Failure(new Error(ResponseMessages.ExerciseHistoryNotFound));
        }

        var setHistory = SetHistory.Create(exerciseHistory.Id, command.Reps, command.Weight);
        exerciseHistory.AddSetHistory(setHistory);

        await _context.SaveChangesAsync();

        var setDto = new SetDto(setHistory.Id, setHistory.Reps, setHistory.Weight);
            
        return Result<SetDto>.Success(setDto);
    }
}