using PostService.Application.DTOs;
using PostService.Application.Queries.GetAllPosts;
using PostService.Application.Queries.GetPost;
using PostService.Domain.Entities;

namespace PostService.Api.GraphQL.Queries;

public class PostQuery
{
    public async Task<PostDto> GetPost(Guid id, [Service] GetPostQueryHandler getPostQueryHandler)
    {
        var query = new GetPostQuery(id);

        var result = await getPostQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<IList<Post>> GetAllPost(int first, Guid lastPostId, [Service] GetAllPostsQueryHandler getAllPostsQueryHandler)
    {
        var query = new GetAllPostsQuery(first, lastPostId);

        var result = await getAllPostsQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}