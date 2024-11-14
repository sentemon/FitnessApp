using Microsoft.EntityFrameworkCore;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.DeleteLike;

public class DeleteLikeCommandHandler : ICommandHandler<DeleteLikeCommand, string>
{
    private readonly PostDbContext _context;

    public DeleteLikeCommandHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteLikeCommand command)
    {
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.Id == command.Id);

        if (like == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.LikeNotFound));
        }

        var isAlreadyLiked = await _context.Likes
            .AnyAsync(l => l.PostId == command.PostId && l.UserId == command.UserId);
    
        if (!isAlreadyLiked)
        {
            return Result<string>.Failure(new Error(ResponseMessages.UserHasNotLikedThisPostYet));
        }

        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == like.PostId);

        if (post == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        post.DecrementLikeCount();
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();

        return Result<string>.Success(ResponseMessages.YouSuccessfullyDeletedLike);
    }
}