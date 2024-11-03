using Microsoft.EntityFrameworkCore;
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
            return Result<string>.Failure(new Error("Comment not found."));
        }

        if (comment.UserId != command.UserId)
        {
            return Result<string>.Failure(new Error("You do not have permission to delete this comment."));
        }

        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == comment.PostId);

        if (post == null)
        {
            return Result<string>.Failure(new Error("Post not found."));
        }
        
        post.DecrementCommentCount();
        _context.Remove(comment);
        await _context.SaveChangesAsync();

        return Result<string>.Success("You successfully deleted comment.");
    }
}