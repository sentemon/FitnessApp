using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostService.Application.DTOs;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.UpdatePost;

public class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, PostDto>
{
    private readonly PostDbContext _context;
    private readonly ILogger<UpdatePostCommandHandler> _logger;

    public UpdatePostCommandHandler(PostDbContext context, ILogger<UpdatePostCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<PostDto, Error>> HandleAsync(UpdatePostCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == Guid.Parse(command.UpdatePost.Id));

        if (post == null)
        {
            _logger.LogWarning("Attempted to update a post that does not exist: PostId: {PostId}", command.UpdatePost.Id);
            return Result<PostDto>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        if (post.UserId != command.UserId)
        {
            _logger.LogWarning("User {UserId} attempted to update a post they do not own: PostId: {PostId}", command.UserId, command.UpdatePost.Id);
            return Result<PostDto>.Failure(new Error(ResponseMessages.YouDoNotHavePermissionToUpdateThisPost));
        }

        var user = await _context.Users.FirstAsync(u => u.Id == post.UserId);
        
        post.Update(command.UpdatePost.Title, command.UpdatePost.Description);
        await _context.SaveChangesAsync();
        
        var postDto = new PostDto(
            post.Id,
            post.Title,
            post.Description,
            post.ContentUrl,
            post.ContentType,
            post.LikeCount,
            post.CommentCount,
            post.CreatedAt,
            user.ImageUrl,
            user.Username
        );

        return Result<PostDto>.Success(postDto);
    }
}