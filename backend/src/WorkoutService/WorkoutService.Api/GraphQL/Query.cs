using System.Security.Claims;
using WorkoutService.Application.DTOs;
using WorkoutService.Application.Queries.GetAllWorkouts;

namespace WorkoutService.Api.GraphQL;

public class Query
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Query(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<WorkoutDto>> GetAllWorkouts([Service] GetAllWorkoutsQueryHandler getAllWorkoutsQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new GetAllWorkoutsQuery(userId);
        var result = await getAllWorkoutsQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}