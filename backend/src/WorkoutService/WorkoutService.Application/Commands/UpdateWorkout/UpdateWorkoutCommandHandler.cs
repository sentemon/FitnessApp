using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.Application.Extensions;
using WorkoutService.Application.DTOs;
using WorkoutService.Application.Validators;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.UpdateWorkout;

public class UpdateWorkoutCommandHandler : ICommandHandler<UpdateWorkoutCommand, string>
{
    private readonly WorkoutDbContext _context;

    private readonly IValidator<UpdateWorkoutDto> _validator;

    public UpdateWorkoutCommandHandler(WorkoutDbContext context)
    {
        _context = context;

        _validator = new UpdateWorkoutValidator();
    }

    public async Task<IResult<string, Error>> HandleAsync(UpdateWorkoutCommand command)
    {
        var errors = await _validator.ValidateResultAsync(command.UpdateWorkoutDto);
        if (errors is not null)
        {
            return Result<string>.Failure(new Error(errors));
        }

        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == command.UpdateWorkoutDto.Id);
        if (workout is null)
        {
            return Result<string>.Failure(new Error("Workout not found."));
        }
        
        workout.Update(
            command.UpdateWorkoutDto.Title,
            command.UpdateWorkoutDto.Description,
            command.UpdateWorkoutDto.DurationInMinutes,
            command.UpdateWorkoutDto.Level
        );
        
        // ToDo: get from File Service
        workout.SetImageUrl("https://example.com/image");

        await _context.SaveChangesAsync();
        
        return Result<string>.Success("Workout is successfully updated");
    }
}