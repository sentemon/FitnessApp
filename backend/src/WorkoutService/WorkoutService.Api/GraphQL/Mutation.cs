using System.Security.Claims;
using WorkoutService.Application.Commands.AddSet;
using WorkoutService.Application.Commands.AddSetHistory;
using WorkoutService.Application.Commands.AddWorkoutHistory;
using WorkoutService.Application.Commands.CompleteWorkoutHistory;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteSet;
using WorkoutService.Application.Commands.DeleteSetHistory;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.Commands.MarkSetHistoryAsCompleted;
using WorkoutService.Application.Commands.MarkSetHistoryAsUncompleted;
using WorkoutService.Application.Commands.SetUpProfile;
using WorkoutService.Application.Commands.UpdateWholeWorkout;
using WorkoutService.Application.Commands.UpdateWorkout;
using WorkoutService.Application.DTOs;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Api.GraphQL;

public class Mutation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Mutation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> SetUpProfile(SetUpProfileDto input, [Service] SetUpProfileCommandHandler setUpProfileCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new SetUpProfileCommand(input, userId);
        var result = await setUpProfileCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
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

    public async Task<SetDto> AddSet(uint reps, int weight, string exerciseId, [Service] AddSetCommandHandler addSetCommandHandler)
    {
        var command = new AddSetCommand(reps, weight, Guid.Parse(exerciseId));
        var result = await addSetCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<string> DeleteSet(string id, string exerciseId, [Service] DeleteSetCommandHandler deleteSetCommandHandler)
    {
        var command = new DeleteSetCommand(Guid.Parse(id), Guid.Parse(exerciseId));
        var result = await deleteSetCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<SetDto> AddSetHistory(uint reps, int weight, string exerciseHistoryId, [Service] AddSetHistoryCommandHandler addSetHistoryCommandHandler)
    {
        var command = new AddSetHistoryCommand(reps, weight, Guid.Parse(exerciseHistoryId));
        var result = await addSetHistoryCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> DeleteSetHistory(string id, string exerciseHistoryId, [Service] DeleteSetHistoryCommandHandler deleteSetHistoryCommandHandler)
    {
        var command = new DeleteSetHistoryCommand(Guid.Parse(id), Guid.Parse(exerciseHistoryId));
        var result = await deleteSetHistoryCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<string> MarkSetHistoryAsCompleted(string id, string exerciseHistoryId, [Service] MarkSetHistoryAsCompletedCommandHandler markSetHistoryAsCompletedCommandHandler)
    {
        var command = new MarkSetHistoryAsCompletedCommand(Guid.Parse(id), Guid.Parse(exerciseHistoryId));
        var result = await markSetHistoryAsCompletedCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> MarkSetHistoryAsUncompleted(string id, string exerciseHistoryId, [Service] MarkSetHistoryAsUncompletedCommandHandler markSetHistoryAsUncompletedCommandHandler)
    {
        var command = new MarkSetHistoryAsUncompletedCommand(Guid.Parse(id), Guid.Parse(exerciseHistoryId));
        var result = await markSetHistoryAsUncompletedCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }

    public async Task<WorkoutHistory> AddWorkoutHistory(string workoutId, [Service] AddWorkoutHistoryCommandHandler addWorkoutHistoryCommandHandler)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var command = new AddWorkoutHistoryCommand(Guid.Parse(workoutId), userId);
        var result = await addWorkoutHistoryCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
    
    public async Task<string> CompleteWorkoutHistory(string id, uint durationInMinutes, [Service] CompleteWorkoutHistoryCommandHandler completeWorkoutHistoryCommandHandler)
    {
        var command = new CompleteWorkoutHistoryCommand(Guid.Parse(id), durationInMinutes);
        var result = await completeWorkoutHistoryCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new GraphQLException(new Error(result.Error.Message));
        }

        return result.Response;
    }
}