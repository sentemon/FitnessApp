using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostService.Application.DTOs;
using PostService.Domain.Constants;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.AddLike;

public class AddLikeCommandHandler : ICommandHandler<AddLikeCommand, LikeDto>
{
    private readonly PostDbContext _context;
    private readonly ILogger<AddLikeCommandHandler> _logger;

    public AddLikeCommandHandler(PostDbContext context, ILogger<AddLikeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<LikeDto, Error>> HandleAsync(AddLikeCommand command)
    {
        if (command.UserId == null)
        {
            _logger.LogWarning("Attempted to like a post with a null UserId.");
            return Result<LikeDto>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == command.PostId);

        if (post == null)
        {
            _logger.LogWarning("Attempted to like a post that does not exist: PostId: {PostId}", command.PostId);
            return Result<LikeDto>.Failure(new Error(ResponseMessages.PostNotFound));
        }
        
        var isAlreadyLiked = await _context.Likes
            .AnyAsync(l => l.PostId == command.PostId && l.UserId == command.UserId);
    
        if (isAlreadyLiked)
        {
            _logger.LogWarning("User {UserId} has already liked post {PostId}.", command.UserId, command.PostId);
            return Result<LikeDto>.Failure(new Error(ResponseMessages.UserHasAlreadyLikedThisPost));
        }
            
        var like = new Like(command.PostId, command.UserId);
        
        _context.Likes.Add(like);
        
        post.IncrementLikeCount();

        await _context.SaveChangesAsync();

        var likeDto = new LikeDto(
            like.Id,
            like.PostId,
            like.UserId,
            like.CreatedAt
        );

        return Result<LikeDto>.Success(likeDto);
    }
}
