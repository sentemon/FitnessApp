using System.Security.Claims;
using PostService.Application.Commands.AddComment;
using PostService.Application.Commands.DeleteComment;
using PostService.Application.DTOs;

namespace PostService.Api.GraphQL.Mutations;

public class CommentMutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentMutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentDto> CreateComment(CreateCommentDto input, [Service] AddCommentCommandHandler addCommentCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new AddCommentCommand(input, userId);

        var result = await addCommentCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> DeleteComment(Guid id, Guid postId, [Service] DeleteCommentCommandHandler deleteCommentCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new DeleteCommentCommand(id, postId, userId);

        var result = await deleteCommentCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}