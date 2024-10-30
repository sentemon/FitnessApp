using PostService.Domain.Entities;
using Shared.Application.Abstractions;

namespace PostService.Application.Queries.GetPost;

public class GetPostQueryHandler : IQueryHandler<GetPostQuery, Post>
{
    public async Task<Post> HandleAsync(GetPostQuery query)
    {
        throw new NotImplementedException();
    }
}