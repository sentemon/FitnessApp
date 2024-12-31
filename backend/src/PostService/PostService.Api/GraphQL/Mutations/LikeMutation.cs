using System.Security.Claims;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.DeleteLike;
using PostService.Application.DTOs;

namespace PostService.Api.GraphQL.Mutations;

public class LikeMutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LikeMutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LikeDto> AddLike(Guid postId, [Service] AddLikeCommandHandler addLikeCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new AddLikeCommand(postId, userId);

        var result = await addLikeCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> DeleteLike(Guid id, Guid postId, [Service] DeleteLikeCommandHandler deleteLikeCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new DeleteLikeCommand(id, postId, userId);

        var result = await deleteLikeCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}