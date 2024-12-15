using AuthService.Application.DTOs;
using AuthService.Infrastructure.Interfaces;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(GetUserByIdQuery query)
    {
        var user = await _userService.GetByIdAsync(query.Id);

        if (user == null)
        {
            return Result<UserDto>.Failure(new Error("User not found."));
        }

        var userDto = new UserDto(user.FirstName,
            user.LastName,
            user.Username,
            user.Email,
            user.ImageUrl);

        return Result<UserDto>.Success(userDto);
    }
}