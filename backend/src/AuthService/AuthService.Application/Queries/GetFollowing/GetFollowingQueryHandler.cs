using AuthService.Application.Queries.GetFollowers;
using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetFollowing;

public class GetFollowingQueryHandler : IQueryHandler<GetFollowingQuery, ICollection<User>>
{
    private readonly AuthDbContext _context;

    public GetFollowingQueryHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<ICollection<User>, Error>> HandleAsync(GetFollowingQuery query)
    {
        var user = await _context.Users
            .Include(u => u.Following)
                .ThenInclude(f => f.Following)
            .FirstOrDefaultAsync(u => u.Id == query.UserId);

        if (user is null)
        {
            return Result<ICollection<User>>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var following = user.Following.Select(f => f.Following).ToList();
            
        return Result<ICollection<User>>.Success(following);
    }
}