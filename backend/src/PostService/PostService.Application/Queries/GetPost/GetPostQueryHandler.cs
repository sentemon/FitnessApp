using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<GetPostQueryHandler> _logger;

    public GetPostQueryHandler(PostDbContext context, ILogger<GetPostQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<PostDto, Error>> HandleAsync(GetPostQuery query)
    {
        var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == query.Id);

        if (post == null)
        {
            _logger.LogWarning("Attempted to retrieve a post that does not exist: PostId: {PostId}", query.Id);
            return Result<PostDto>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        var user = await _context.Users.AsNoTracking().FirstAsync(u => u.Id  == post.UserId);
        
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