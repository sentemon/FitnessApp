using FluentValidation;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Application.Validators;

public class SetUpProfileValidator : AbstractValidator<SetUpProfileDto>
{
    public SetUpProfileValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow).WithMessage("Date of Birth must be in the past.");

        RuleFor(x => x.Goal)
            .IsInEnum().WithMessage("Invalid goal specified.");

        RuleFor(x => x.ActivityLevel)
            .IsInEnum().WithMessage("Invalid activity level specified.");
    }
}