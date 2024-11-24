using Microsoft.EntityFrameworkCore;
using PostService.Application.DTOs;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.UpdatePost;

public class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, PostDto>
{
    private readonly PostDbContext _context;

    public UpdatePostCommandHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<PostDto, Error>> HandleAsync(UpdatePostCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == command.UpdatePost.Id);

        if (post == null)
        {
            return Result<PostDto>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        if (post.UserId != command.UserId)
        {
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