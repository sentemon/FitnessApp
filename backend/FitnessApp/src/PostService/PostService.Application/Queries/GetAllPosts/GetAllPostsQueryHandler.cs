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
        var lastPostId = Guid.Empty;
        
        if (!string.IsNullOrEmpty(query.AfterCursor) && !Guid.TryParse(query.AfterCursor, out lastPostId))
        {
            return new Result<IList<Post>>(new Error("Invalid cursor format."));
        }
        
        var queryablePosts = _context.Posts.AsQueryable();

        if (lastPostId != Guid.Empty)
        {
            queryablePosts = queryablePosts.Where(p => p.Id != lastPostId);
        }
        
        var posts = await queryablePosts
            .OrderBy(p => p.CreatedAt)
            .Take(query.First)
            .ToListAsync();
        
        return new Result<IList<Post>>(posts);
    }
}