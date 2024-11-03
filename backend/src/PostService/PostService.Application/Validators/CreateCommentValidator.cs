using FluentValidation;
using PostService.Application.DTOs;

namespace PostService.Application.Validators;

public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentValidator()
    {
        RuleFor(c => c.Content)
            .NotNull().WithMessage("Content is required.")
            .MaximumLength(512).WithMessage("Comment cannot be longer than 512 characters");
    }
}