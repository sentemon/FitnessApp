using Microsoft.EntityFrameworkCore;
using PostService.Application.DTOs;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.GetAllPosts;

public class GetAllPostsQueryHandler : IQueryHandler<GetAllPostsQuery, IList<PostDto>>
{
    private readonly PostDbContext _context;

    public GetAllPostsQueryHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<IList<PostDto>, Error>> HandleAsync(GetAllPostsQuery query)
    {
        var queryablePosts = _context.Posts.AsNoTracking().AsQueryable();

        if (query.LastPostId != Guid.Empty)
        {
            queryablePosts = queryablePosts.Where(p => p.Id != query.LastPostId);
        }
        
        var postDtos = await queryablePosts
            .OrderBy(p => p.CreatedAt)
            .Take(query.First)
            .Join(
                _context.Users,
                post => post.UserId,
                user => user.Id,
                (post, user) => new PostDto(
                
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
                ))
            .ToListAsync();
        
        return Result<IList<PostDto>>.Success(postDtos);
    }
}