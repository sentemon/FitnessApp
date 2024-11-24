using Microsoft.EntityFrameworkCore;
using PostService.Application.DTOs;
using PostService.Domain.Constants;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.GetPost;

public class GetPostQueryHandler : IQueryHandler<GetPostQuery, PostDto>
{
    private readonly PostDbContext _context;

    public GetPostQueryHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<PostDto, Error>> HandleAsync(GetPostQuery query)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == query.Id);

        if (post == null)
        {
            return Result<PostDto>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        var user = await _context.Users.FirstAsync(u => u.Id  == post.UserId);
        
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