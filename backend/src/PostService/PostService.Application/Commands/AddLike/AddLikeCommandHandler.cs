using Microsoft.EntityFrameworkCore;
using PostService.Application.DTOs;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.AddLike;

public class AddLikeCommandHandler : ICommandHandler<AddLikeCommand, LikeDto>
{
    private readonly PostDbContext _context;

    public AddLikeCommandHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<LikeDto, Error>> HandleAsync(AddLikeCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == command.PostId);

        if (post == null)
        {
            return Result<LikeDto>.Failure(new Error("Post not found."));
        }

        if (post.UserId != command.UserId)
        {
            return Result<LikeDto>.Failure(new Error("You do not have permission to like this post."));
        }
            
        var like = new Like(
            command.PostId, 
            command.UserId);
        
        _context.Likes.Add(like);
        
        post.IncrementLikeCount();

        await _context.SaveChangesAsync();

        var likeDto = new LikeDto(
            like.PostId,
            like.UserId,
            like.CreatedAt);

        return Result<LikeDto>.Success(likeDto);
    }
}
