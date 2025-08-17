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

namespace WorkoutService.Application.Commands.SetUpProfile;

public class SetUpProfileCommandHandler : ICommandHandler<SetUpProfileCommand, string>
{
    private readonly WorkoutDbContext _context;
    private readonly ILogger<SetUpProfileCommandHandler> _logger;
    
    private readonly IValidator<SetUpProfileDto> _validator;

    public SetUpProfileCommandHandler(WorkoutDbContext context, ILogger<SetUpProfileCommandHandler> logger)
    {
        _context = context;
        _logger = logger;

        _validator = new SetUpProfileValidator();
    }

    public async Task<IResult<string, Error>> HandleAsync(SetUpProfileCommand command)
    {
        var errors = await _validator.ValidateResultAsync(command.SetUpProfileDto);
        if (errors is not null)
        {
            _logger.LogWarning("Validation failed for SetUpProfileCommand: {Errors}", errors);
            return Result<string>.Failure(new Error(errors));
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user is null)
        {
            _logger.LogWarning("Attempted to set up a profile for a user that does not exist: UserId: {UserId}", command.UserId);
            return Result<string>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        user.SetUpProfile(
            command.SetUpProfileDto.Weight,
            command.SetUpProfileDto.Height,
            command.SetUpProfileDto.Goal,
            command.SetUpProfileDto.ActivityLevel,
            command.SetUpProfileDto.FavoriteWorkoutTypes,
            command.SetUpProfileDto.DateOfBirth
        );

        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.ProfileSetUp);
    }
}