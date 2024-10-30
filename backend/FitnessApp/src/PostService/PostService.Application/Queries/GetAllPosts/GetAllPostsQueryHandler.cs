using PostService.Domain.Entities;
using Shared.Application.Abstractions;

namespace PostService.Application.Queries.GetAllPosts;

public class GetAllPostsQueryHandler : IQueryHandler<GetAllPostsQuery, IQueryable<Post>>
{
    public Task<IQueryable<Post>> HandleAsync(GetAllPostsQuery query)
    {
        throw new NotImplementedException();
    }
}