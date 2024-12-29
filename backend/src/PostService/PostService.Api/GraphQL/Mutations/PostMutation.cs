using System.Security.Claims;
using PostService.Application.Commands.AddPost;
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
}