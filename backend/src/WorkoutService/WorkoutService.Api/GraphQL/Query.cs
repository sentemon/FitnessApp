using System.Security.Claims;
using WorkoutService.Application.DTOs;
using WorkoutService.Application.Queries.GetAllWorkouts;
using WorkoutService.Application.Queries.ProfileSetUp;

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

    public async Task<bool> ProfileSetUp([Service] ProfileSetUpQueryHandler profileSetUpQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new ProfileSetUpQuery(userId);
        var result = await profileSetUpQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}