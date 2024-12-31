using PostService.Application.Queries.GetAllLikes;
using PostService.Domain.Entities;

namespace PostService.Api.GraphQL.Queries;

public class LikeQuery
{
    public async Task<IList<Like>> GetAllLikes(Guid postId, int first, [Service] GetAllLikesQueryHandler getAllLikesQueryHandler)
    {
        var query = new GetAllLikesQuery(postId, first);

        var result = await getAllLikesQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}