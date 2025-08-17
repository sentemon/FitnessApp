using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AddCommentCommandHandler> _logger;

    public AddCommentCommandHandler(PostDbContext context, ILogger<AddCommentCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _validator = new CreateCommentValidator();
    }

    public async Task<IResult<CommentDto, Error>> HandleAsync(AddCommentCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == Guid.Parse(command.CreateComment.PostId));
        
        if (post is null)
        {
            _logger.LogWarning("Attempted to add a comment to a post that does not exist: PostId: {PostId}", command.CreateComment.PostId);
            return Result<CommentDto>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);

        if (user is null)
        {
            _logger.LogWarning("Attempted to add a comment with a user that does not exist: UserId: {UserId}", command.UserId);
            return Result<CommentDto>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        var errorMessage = await _validator.ValidateResultAsync(command.CreateComment);

        if (errorMessage is not null)
        {
            _logger.LogWarning("Validation failed for AddCommentCommand: {ErrorMessage}", errorMessage);
            return Result<CommentDto>.Failure(new Error(errorMessage));
        }
        
        var comment = new Comment(
            post.Id,
            post.UserId,
            user.Username,
            command.CreateComment.Content
        );

        _context.Comments.Add(comment);
        post.IncrementCommentCount();
        await _context.SaveChangesAsync();

        var commentDto = new CommentDto(
            comment.Id,
            comment.PostId,
            comment.UserId,
            comment.Username,
            comment.Content,
            comment.CreatedAt
        );

        return Result<CommentDto>.Success(commentDto);
    }
}