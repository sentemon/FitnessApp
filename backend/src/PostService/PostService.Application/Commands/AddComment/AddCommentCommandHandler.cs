using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostService.Application.DTOs;
using PostService.Application.Validators;
using PostService.Domain.Constants;
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

    public AddCommentCommandHandler(PostDbContext context)
    {
        _context = context;
        _validator = new CreateCommentValidator();
    }

    public async Task<IResult<CommentDto, Error>> HandleAsync(AddCommentCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == command.CreateComment.PostId);
        
        if (post == null)
        {
            return Result<CommentDto>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user == null)
        {
            return Result<CommentDto>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var errorMessage = await _validator.ValidateResultAsync(command.CreateComment);

        if (errorMessage != null)
        {
            return Result<CommentDto>.Failure(new Error(errorMessage));
        }
        
        var comment = new Comment(
            command.CreateComment.PostId,
            command.UserId,
            user.Username,
            command.CreateComment.Content);

        _context.Comments.Add(comment);
        post.IncrementCommentCount();
        await _context.SaveChangesAsync();

        var commentDto = new CommentDto(
            comment.Id,
            comment.PostId,
            comment.UserId,
            comment.Username,
            comment.Content,
            comment.CreatedAt);

        return Result<CommentDto>.Success(commentDto);
    }
}