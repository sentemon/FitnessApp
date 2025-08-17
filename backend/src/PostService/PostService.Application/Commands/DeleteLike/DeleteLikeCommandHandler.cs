using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.DeleteLike;

public class DeleteLikeCommandHandler : ICommandHandler<DeleteLikeCommand, string>
{
    private readonly PostDbContext _context;
    private readonly ILogger<DeleteLikeCommandHandler> _logger;

    public DeleteLikeCommandHandler(PostDbContext context, ILogger<DeleteLikeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeleteLikeCommand command)
    {
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == command.PostId);

        if (like == null)
        {
            _logger.LogWarning("Attempted to delete a like for a post that does not exist: PostId: {PostId}", command.PostId);
            return Result<string>.Failure(new Error(ResponseMessages.LikeNotFound));
        }

        var isAlreadyLiked = await _context.Likes
            .AnyAsync(l => l.PostId == command.PostId && l.UserId == command.UserId);
    
        if (!isAlreadyLiked)
        {
            _logger.LogWarning("User {UserId} has not liked post {PostId} yet.", command.UserId, command.PostId);
            return Result<string>.Failure(new Error(ResponseMessages.UserHasNotLikedThisPostYet));
        }

        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == like.PostId);

        if (post == null)
        {
            _logger.LogWarning("Attempted to delete a like for a post that does not exist: PostId: {PostId}", like.PostId);
            return Result<string>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        post.DecrementLikeCount();
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();

        return Result<string>.Success(ResponseMessages.YouSuccessfullyDeletedLike);
    }
}