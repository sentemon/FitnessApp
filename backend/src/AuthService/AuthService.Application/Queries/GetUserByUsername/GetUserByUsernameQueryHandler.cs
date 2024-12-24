using AuthService.Application.DTOs;
using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler : IQueryHandler<GetUserByUsernameQuery, UserDto>
{
    private readonly AuthDbContext _context;

    public GetUserByUsernameQueryHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(GetUserByUsernameQuery query)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Value == query.Username);

        if (user == null)
        {
            return Result<UserDto>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var userDto = new UserDto(
            user.FirstName,
            user.LastName,
            user.Username.Value,
            string.Empty,
            user.ImageUrl
        );

        return Result<UserDto>.Success(userDto);
    }
}