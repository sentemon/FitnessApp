using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.Application.Extensions;
using WorkoutService.Application.DTOs;
using WorkoutService.Application.Validators;
using WorkoutService.Domain.Constants;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Commands.UpdateWorkout;

public class UpdateWorkoutCommandHandler : ICommandHandler<UpdateWorkoutCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<UpdateWorkoutCommandHandler> _logger;

    private readonly IValidator<UpdateWorkoutDto> _validator;

    public UpdateWorkoutCommandHandler(WorkoutDbContext context, ILogger<UpdateWorkoutCommandHandler> logger)
    {
        _context = context;
        _logger = logger;

        _validator = new UpdateWorkoutValidator();
    }

    public async Task<IResult<string, Error>> HandleAsync(UpdateWorkoutCommand command)
    {
        var errors = await _validator.ValidateResultAsync(command.UpdateWorkoutDto);
        if (errors is not null)
        {
            _logger.LogWarning("Validation failed for UpdateWorkoutCommand: {Errors}", errors);
            return Result<string>.Failure(new Error(errors));
        }

        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == command.UpdateWorkoutDto.Id);
        if (workout is null)
        {
            _logger.LogWarning("Attempted to update a workout that does not exist: WorkoutId: {WorkoutId}", command.UpdateWorkoutDto.Id);
            return Result<string>.Failure(new Error(ResponseMessages.WorkoutNotFound));
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
        
        return Result<string>.Success(ResponseMessages.WorkoutUpdated);
    }
}