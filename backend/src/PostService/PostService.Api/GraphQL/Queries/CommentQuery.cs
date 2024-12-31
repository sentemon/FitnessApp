using PostService.Application.Queries.GetAllComments;
using PostService.Domain.Entities;

namespace PostService.Api.GraphQL.Queries;

public class CommentQuery
{
    public async Task<IList<Comment>> GetAllComments(Guid postId, int first, [Service] GetAllCommentsQueryHandler getAllCommentsQueryHandler)
    {
        var query = new GetAllCommentsQuery(postId, first);

        var result = await getAllCommentsQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}