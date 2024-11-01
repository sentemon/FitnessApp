using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.GetPost;

public class GetPostQueryHandler : IQueryHandler<GetPostQuery, Post>
{
    private readonly PostDbContext _context;

    public GetPostQueryHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<Post, Error>> HandleAsync(GetPostQuery query)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == query.Id);

        if (post == null)
        {
            return new Result<Post>(new Error("Post not found."));
        }

        return new Result<Post>(post);
    }
}