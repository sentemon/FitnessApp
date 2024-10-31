using PostService.Domain.Entities;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.GetPost;

public class GetPostQueryHandler : IQueryHandler<GetPostQuery, Post>
{
    public async Task<IResult<Post, Error>> HandleAsync(GetPostQuery query)
    {
        throw new NotImplementedException();
    }
}