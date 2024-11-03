using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.GetAllLikes;

public class GetAllLikesQueryHandler : IQueryHandler<GetAllLikesQuery, IList<Like>>
{
    private readonly PostDbContext _context;

    public GetAllLikesQueryHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<IList<Like>, Error>> HandleAsync(GetAllLikesQuery query)
    {
        var queryableLikes = _context.Likes
            .AsQueryable()
            .Where(l => l.PostId == query.PostId);
        
        var likes = await queryableLikes
            .OrderBy(p => p.CreatedAt)
            .Take(query.First)
            .ToListAsync();

        return Result<IList<Like>>.Success(likes);
    }
}