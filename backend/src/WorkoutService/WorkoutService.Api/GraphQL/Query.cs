using System.Security.Claims;
using WorkoutService.Application.DTOs;
using WorkoutService.Application.Queries.GetAllWorkoutHistories;
using WorkoutService.Application.Queries.GetAllWorkouts;
using WorkoutService.Application.Queries.GetWorkoutByUrl;
using WorkoutService.Application.Queries.ProfileSetUp;
using WorkoutService.Domain.Entities;

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
    
    public async Task<List<WorkoutHistory>> GetAllWorkoutHistories([Service] GetAllWorkoutHistoriesQueryHandler getAllWorkoutHistoriesQueryHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new GetAllWorkoutHistoriesQuery(userId);
        var result = await getAllWorkoutHistoriesQueryHandler.HandleAsync(query);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<WorkoutDto> GetWorkoutByUrl(string url, [Service] GetWorkoutByUrlQueryHandler getWorkoutByUrlQueryHandler)
    {
        var query = new GetWorkoutByUrlQuery(url);
        var result = await getWorkoutByUrlQueryHandler.HandleAsync(query);

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