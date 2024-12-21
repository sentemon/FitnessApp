using System.Security.Claims;
using AuthService.Application.DTOs;
using AuthService.Application.Queries.GetUserById;

namespace AuthService.Api.GraphQL;

public class Query
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Query(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserDto> GetUserById([Service] GetUserByIdQueryHandler getUserByIdQueryHandler)
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
}