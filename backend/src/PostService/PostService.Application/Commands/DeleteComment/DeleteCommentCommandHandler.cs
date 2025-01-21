using Microsoft.EntityFrameworkCore;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.DeleteComment;

public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand, string>
{
    private readonly PostDbContext _context;

    public DeleteCommentCommandHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteCommentCommand command)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == command.Id);

        if (comment == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.CommentNotFound));
        }

        if (comment.UserId != command.UserId)
        {
            return Result<string>.Failure(new Error(ResponseMessages.YouDoNotHavePermissionToDeleteThisComment));
        }
        
        var post = await _context.Posts.FirstAsync(p => p.Id == comment.PostId);
        
        post.DecrementCommentCount();
        _context.Remove(comment);
        await _context.SaveChangesAsync();

        return Result<string>.Success(ResponseMessages.YouSuccessfullyDeletedComment);
    }
}