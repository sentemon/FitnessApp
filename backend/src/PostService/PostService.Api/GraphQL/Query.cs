using PostService.Application.DTOs;
using PostService.Application.Queries.GetAllComments;
using PostService.Application.Queries.GetAllLikes;
using PostService.Application.Queries.GetAllPosts;
using PostService.Application.Queries.GetPost;
using PostService.Domain.Entities;

namespace PostService.Api.GraphQL;

public class Query
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
    
    public async Task<IList<Post>> GetAllPost(int first, string lastPostId, [Service] GetAllPostsQueryHandler getAllPostsQueryHandler)
    {
        var query = new GetAllPostsQuery(first, Guid.Parse(lastPostId));

        var result = await getAllPostsQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
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

    public string Test() => "Hello World";
}