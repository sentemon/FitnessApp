using AuthService.Application.DTOs;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.SearchUsers;

public class SearchUsersQueryHandler : IQueryHandler<SearchUsersQuery, IEnumerable<UserDto>>
{
    private readonly AuthDbContext _context;

    public SearchUsersQueryHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<IEnumerable<UserDto>, Error>> HandleAsync(SearchUsersQuery query)
    {
        var search = $"%{query.Search}%";

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id != query.UserId)   
            .Where(u =>
                EF.Functions.Like(u.Username.Value, search) ||
                EF.Functions.Like(u.FirstName + " " + u.LastName, search))
            .OrderBy(u => u.Username.Value)
            .Take(10)
            .Select(u => new UserDto(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Username.Value,
                "",
                u.ImageUrl,
                u.FollowingCount,
                u.FollowersCount
            ))
            .ToListAsync();

        return Result<IEnumerable<UserDto>>.Success(users);
    }
}