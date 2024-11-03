using FluentValidation;
using PostService.Application.DTOs;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.Application.Extensions;

namespace PostService.Application.Commands.AddComment;

public class AddCommentCommandHandler : ICommandHandler<AddCommentCommand, CommentDto>
{
    private readonly PostDbContext _context;
    private readonly IValidator<CreateCommentDto> _validator;

    public AddCommentCommandHandler(PostDbContext context, IValidator<CreateCommentDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IResult<CommentDto, Error>> HandleAsync(AddCommentCommand command)
    {
        var errorMessage = await _validator.ValidateResultAsync(command.CreateComment);

        if (errorMessage != null)
        {
            return Result<CommentDto>.Failure(new Error(errorMessage));
        }

        var comment = new Comment(
            command.CreateComment.PostId,
            command.UserId,
            command.CreateComment.Content);

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var commentDto = new CommentDto(
            comment.Id,
            comment.UserId,
            comment.Content,
            comment.CreatedAt);

        return Result<CommentDto>.Success(commentDto);
    }
}