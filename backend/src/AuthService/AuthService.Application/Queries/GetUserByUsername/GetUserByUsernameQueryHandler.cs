using AuthService.Application.DTOs;
using AuthService.Domain.Constants;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler : IQueryHandler<GetUserByUsernameQuery, UserDto>
{
    private readonly AuthDbContext _context;
    private readonly ILogger<GetUserByUsernameQueryHandler> _logger;

    public GetUserByUsernameQueryHandler(AuthDbContext context, ILogger<GetUserByUsernameQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(GetUserByUsernameQuery query)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username.Value == query.Username);

        if (user == null)
        {
            _logger.LogWarning("Get user by username attempt with non-existing username: {Username}", query.Username);
            return Result<UserDto>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var userDto = new UserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username.Value,
            string.Empty,
            user.Bio,
            user.LastSeenAt,
            user.ImageUrl,
            user.FollowingCount,
            user.FollowersCount
        );

        return Result<UserDto>.Success(userDto);
    }
}