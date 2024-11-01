using FluentValidation;
using PostService.Application.DTOs;
using PostService.Domain.Enums;

namespace PostService.Application.Validators;

public class CreatePostValidator : AbstractValidator<CreatePostDto>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.ContentType)
            .NotNull().WithMessage("ContentType is required.")
            .IsInEnum().WithMessage("ContentType must be one of the following: Text, Image, Video.");

        When(x => x.ContentType == ContentType.Text, () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required for text content.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required for text content.");
        });

        When(x => x.ContentType is ContentType.Image or ContentType.Video, () =>
        {
            RuleFor(x => x.ContentUrl)
                .NotEmpty().WithMessage("ContentUrl is required for video or image content.");
        });
    }
}