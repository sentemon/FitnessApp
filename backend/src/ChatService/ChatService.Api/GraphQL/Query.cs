using System.Security.Claims;
using ChatService.Application.Queries.GetAllChats;
using ChatService.Application.Queries.GetChatById;
using ChatService.Application.Queries.GetLastMessage;
using ChatService.Application.Queries.GetUserById;
using ChatService.Application.Queries.SearchUsers;
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

    public async Task<User> GetCurrentUser([Service] GetUserByIdQueryHandler getUserByIdQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetUserByIdQuery(userId);
        var result = await getUserByIdQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.Error.Message);
        }

        return result.Response;
    }
    
    public async Task<User> GetUserById(string userId, [Service] GetUserByIdQueryHandler getUserByIdQueryHandler)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await getUserByIdQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.Error.Message);
        }

        return result.Response;
    }
    
    public async Task<IEnumerable<User>> SearchUsers(string search, [Service] SearchUsersQueryHandler searchUsersQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new SearchUsersQuery(search, userId);
        var result = await searchUsersQueryHandler.HandleAsync(query);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<Message> GetLastMessage(string chatId, [Service] GetLastMessageQueryHandler getLastMessageQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetLastMessageQuery(Guid.Parse(chatId), userId);
        var result = await getLastMessageQueryHandler.HandleAsync(query);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(result.Error.Message);
        }

        return result.Response;
    }
}