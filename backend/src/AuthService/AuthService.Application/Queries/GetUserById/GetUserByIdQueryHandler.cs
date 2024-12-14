using AuthService.Application.DTOs;
using AuthService.Infrastructure.Interfaces;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IKeycloakService _keycloakService;

    public GetUserByIdQueryHandler(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(GetUserByIdQuery query)
    {
        var user = await _keycloakService.GetUserByIdAsync(query.Id);

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