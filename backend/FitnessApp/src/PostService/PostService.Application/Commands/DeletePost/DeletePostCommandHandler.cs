using Microsoft.EntityFrameworkCore;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.DeletePost;

public class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, Unit>
{
    private readonly PostDbContext _context;

    public DeletePostCommandHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<Unit, Error>> HandleAsync(DeletePostCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == command.Id);

        if (post == null)
        {
            return new Result<Unit>(new Error("Post not found."));
        }

        if (post.UserId != command.UserId)
        {
            return new Result<Unit>(new Error("You do not have permission to delete this post."));
        }
        
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return new Result<Unit>(Unit.Value);
    }
}