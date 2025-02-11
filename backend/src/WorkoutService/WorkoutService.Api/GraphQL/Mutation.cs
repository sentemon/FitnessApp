using System.Security.Claims;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.Commands.UpdateWholeWorkout;
using WorkoutService.Application.Commands.UpdateWorkout;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Api.GraphQL;

public class Mutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Mutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WorkoutDto> CreateWorkout(CreateWorkoutDto input, [Service] CreateWorkoutCommandHandler createWorkoutCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new CreateWorkoutCommand(input, userId);
        var result = await createWorkoutCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> UpdateWorkout(UpdateWorkoutDto input, [Service] UpdateWorkoutCommandHandler updateWorkoutCommandHandler)
    {
        var command = new UpdateWorkoutCommand(input);
        var result = await updateWorkoutCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> UpdateWholeWorkout([Service] UpdateWholeWorkoutCommandHandler updateWholeWorkoutCommandHandler)
    {
        var command = new UpdateWholeWorkoutCommand();
        var result = await updateWholeWorkoutCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> DeleteWorkout(string workoutId, [Service] DeleteWorkoutCommandHandler deleteWorkoutCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new DeleteWorkoutCommand(Guid.Parse(workoutId), userId);
        var result = await deleteWorkoutCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}