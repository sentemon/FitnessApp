using System.Security.Claims;
using AuthService.Application.DTOs;
using AuthService.Application.Queries.GetUserById;
using AuthService.Application.Queries.GetUserByUsername;
using AuthService.Application.Queries.SearchUsers;
using AuthService.Domain.Entities;

namespace AuthService.Api.GraphQL;

public class Query
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Query(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> GetCurrentUser([Service] GetUserByIdQueryHandler getUserByIdQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new GetUserByIdQuery(userId);
        var result = await getUserByIdQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<UserDto> GetUserByUsername(string username, [Service] GetUserByUsernameQueryHandler getUserByUsernameQueryHandler)
    {
        var query = new GetUserByUsernameQuery(username);
        var result = await getUserByUsernameQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<IEnumerable<UserDto>> SearchUsers(string search, [Service] SearchUsersQueryHandler searchUsersQueryHandler)
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
    
    public bool IsAuthenticated() => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}