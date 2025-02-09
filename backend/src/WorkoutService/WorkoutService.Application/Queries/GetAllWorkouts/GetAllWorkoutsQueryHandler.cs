using Shared.Application.Abstractions;
using Shared.Application.Common;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Application.Queries.GetAllWorkouts;

public class GetAllWorkoutsQueryHandler : IQueryHandler<GetAllWorkoutsQuery, List<WorkoutDto>>
{
    public async Task<IResult<List<WorkoutDto>, Error>> HandleAsync(GetAllWorkoutsQuery query)
    {
        throw new NotImplementedException();
    }
}