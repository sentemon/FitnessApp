using Microsoft.EntityFrameworkCore;
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

    public AddSetHistoryCommandHandler(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<SetDto, Error>> HandleAsync(AddSetHistoryCommand command)
    {
        var exerciseHistory = await _context.ExerciseHistories.FirstOrDefaultAsync(e => e.Id == command.ExerciseHistoryId);
        if (exerciseHistory is null)
        {
            return Result<SetDto>.Failure(new Error(ResponseMessages.ExerciseHistoryNotFound));
        }

        var setHistory = SetHistory.Create(exerciseHistory.Id, command.Reps, command.Weight);
        exerciseHistory.AddSetHistory(setHistory);

        await _context.SaveChangesAsync();

        var setDto = new SetDto(setHistory.Id, setHistory.Reps, setHistory.Weight);
            
        return Result<SetDto>.Success(setDto);
    }
}