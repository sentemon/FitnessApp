using System.Security.Claims;
using ChatService.Application.Queries.GetAllChats;
using ChatService.Application.Queries.GetChatById;
using ChatService.Domain.Entities;

namespace ChatService.Api.GraphQL;

public class Query
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Query(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Chat>> GetAllChats([Service] GetAllChatsQueryHandler getAllChatsQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetAllChatsQuery(userId);

        var result = await getAllChatsQueryHandler.HandleAsync(query);
        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.Error.Message);
        }

        return result.Response;
    }

    public async Task<Chat> GetChatById(string chatId, [Service] GetChatByIdQueryHandler getChatByIdQueryHandler)
    {
        var query = new GetChatByIdQuery(Guid.Parse(chatId));

        var result = await getChatByIdQueryHandler.HandleAsync(query);
        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.Error.Message);
        }

        return result.Response;
    }
}