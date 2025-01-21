using System.Security.Claims;
using PostService.Application.Commands.AddComment;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeleteComment;
using PostService.Application.Commands.DeleteLike;
using PostService.Application.Commands.DeletePost;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.DTOs;

namespace PostService.Api.GraphQL;

public class Mutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Mutation(IHttpContextAccessor httpContextAccessor)
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

    public async Task<string> DeletePost(string id, [Service] DeletePostCommandHandler deletePostCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new DeletePostCommand(Guid.Parse(id), userId);

        var result = await deletePostCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
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
    
    public async Task<string> DeleteComment(string id, [Service] DeleteCommentCommandHandler deleteCommentCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new DeleteCommentCommand(Guid.Parse(id), userId);

        var result = await deleteCommentCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<LikeDto> AddLike(string postId, [Service] AddLikeCommandHandler addLikeCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new AddLikeCommand(Guid.Parse(postId), userId);

        var result = await addLikeCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> DeleteLike(string postId, [Service] DeleteLikeCommandHandler deleteLikeCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new DeleteLikeCommand(Guid.Parse(postId), userId);

        var result = await deleteLikeCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}