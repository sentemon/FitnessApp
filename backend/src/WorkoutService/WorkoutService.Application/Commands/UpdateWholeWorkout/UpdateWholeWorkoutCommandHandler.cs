using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace WorkoutService.Application.Commands.UpdateWholeWorkout;

public class UpdateWholeWorkoutCommandHandler : ICommandHandler<UpdateWholeWorkoutCommand, string>
{
    private readonly ILogger<UpdateWholeWorkoutCommandHandler> _logger;

    public UpdateWholeWorkoutCommandHandler(ILogger<UpdateWholeWorkoutCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(UpdateWholeWorkoutCommand command)
    {
        throw new NotImplementedException();
    }
}