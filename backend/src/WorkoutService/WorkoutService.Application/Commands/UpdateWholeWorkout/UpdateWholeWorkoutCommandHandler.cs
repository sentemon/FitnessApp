using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace WorkoutService.Application.Commands.UpdateWholeWorkout;

public class UpdateWholeWorkoutCommandHandler : ICommandHandler<UpdateWholeWorkoutCommand, string>
{
    public async Task<IResult<string, Error>> HandleAsync(UpdateWholeWorkoutCommand command)
    {
        throw new NotImplementedException();
    }
}