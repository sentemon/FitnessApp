using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResult<User, Error>> HandleAsync(GetUserByIdQuery query)
    {
        if (query.Id == null)
        {
            return Result<User>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        var user = await _userService.GetByIdAsync(query.Id);

        if (user == null)
        {
            return Result<User>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        return Result<User>.Success(user);
    }
}