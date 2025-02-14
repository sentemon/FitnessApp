using FluentValidation;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Application.Validators;

public class CreateWorkoutValidator : AbstractValidator<CreateWorkoutDto>
{
    public CreateWorkoutValidator()
    {
        RuleFor(w => w.Title)
            .NotEmpty().WithMessage("Title of workout cannot be empty.")
            .MaximumLength(100).WithMessage("Title of workout cannot be longer than 100 characters.");

        RuleFor(w => w.Description)
            .NotEmpty().WithMessage("Description of workout cannot be empty.")
            .MaximumLength(500).WithMessage("Description of workout cannot be longer than 500 characters.");
        
        RuleFor(w => w.Level)
            .IsInEnum().WithMessage("There is no that level for workout");
    }
}