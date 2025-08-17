using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.DeleteComment;

public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand, string>
{
    private readonly PostDbContext _context;
    private readonly ILogger<DeleteCommentCommandHandler> _logger;

    public DeleteCommentCommandHandler(PostDbContext context, ILogger<DeleteCommentCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteCommentCommand command)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == command.Id);

        if (comment == null)
        {
            _logger.LogWarning("Attempted to delete a comment that does not exist: CommentId: {CommentId}", command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.CommentNotFound));
        }

        if (comment.UserId != command.UserId)
        {
            _logger.LogWarning("User {UserId} attempted to delete a comment they do not own: CommentId: {CommentId}", command.UserId, command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.YouDoNotHavePermissionToDeleteThisComment));
        }
        
        var post = await _context.Posts.FirstAsync(p => p.Id == comment.PostId);
        
        post.DecrementCommentCount();
        _context.Remove(comment);
        await _context.SaveChangesAsync();

        return Result<string>.Success(ResponseMessages.YouSuccessfullyDeletedComment);
    }
}