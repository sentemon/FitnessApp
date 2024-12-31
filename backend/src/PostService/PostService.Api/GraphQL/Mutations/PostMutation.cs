using System.Security.Claims;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeletePost;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.DTOs;

namespace PostService.Api.GraphQL.Mutations;

public class PostMutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostMutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<PostDto> CreatePost(CreatePostDto input, [Service] AddPostCommandHandler addPostCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new AddPostCommand(input, userId);

        var result = await addPostCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<PostDto> UpdatePost(UpdatePostDto input, [Service] UpdatePostCommandHandler updatePostCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new UpdatePostCommand(input, userId);

        var result = await updatePostCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<string> DeletePost(Guid id, [Service] DeletePostCommandHandler deletePostCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new DeletePostCommand(id, userId);

        var result = await deletePostCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}