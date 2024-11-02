using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.GetAllPosts;

public class GetAllPostsQueryHandler : IQueryHandler<GetAllPostsQuery, IList<Post>>
{
    private readonly PostDbContext _context;

    public GetAllPostsQueryHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<IList<Post>, Error>> HandleAsync(GetAllPostsQuery query)
    {
        var queryablePosts = _context.Posts.AsQueryable();

        if (query.LastPostId != Guid.Empty)
        {
            queryablePosts = queryablePosts.Where(p => p.Id != query.LastPostId);
        }
        
        var posts = await queryablePosts
            .OrderBy(p => p.CreatedAt)
            .Take(query.First)
            .ToListAsync();
        
        return Result<IList<Post>>.Success(posts);
    }
}