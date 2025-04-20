using System.Security.Claims;
using ChatService.Application.Commands.CreateChat;
using ChatService.Domain.Entities;

namespace ChatService.Api.GraphQL;

public class Mutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Mutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Chat> CreateChat(string userId, [Service] CreateChatCommandHandler createChatCommandHandler)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var command = new CreateChatCommand(currentUserId, userId);

        var result = await createChatCommandHandler.HandleAsync(command);
        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.Error.Message);
        }

        return result.Response;
    }
}