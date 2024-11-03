using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.GetAllComments;

public class GetAllCommentsQueryHandler : IQueryHandler<GetAllCommentsQuery, IList<Comment>>
{
    private readonly PostDbContext _context;

    public GetAllCommentsQueryHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<IList<Comment>, Error>> HandleAsync(GetAllCommentsQuery query)
    {
        var queryableComments = _context.Comments
            .AsQueryable()
            .Where(c => c.PostId == query.PostId);

        var comments = await queryableComments
            .OrderBy(p => p.CreatedAt)
            .Take(query.First)
            .ToListAsync();

        return Result<IList<Comment>>.Success(comments);
    }
}