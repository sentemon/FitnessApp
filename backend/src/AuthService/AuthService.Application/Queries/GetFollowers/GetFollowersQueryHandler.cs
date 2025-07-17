using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetFollowers;

public class GetFollowersQueryHandler : IQueryHandler<GetFollowersQuery, ICollection<User>>
{
    private readonly AuthDbContext _context;

    public GetFollowersQueryHandler(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<ICollection<User>, Error>> HandleAsync(GetFollowersQuery query)
    {
        var user = await _context.Users
            .Include(u => u.Username)
            .Include(u => u.Followers)
                .ThenInclude(f => f.Follower)
            .FirstOrDefaultAsync(u => u.Id == query.UserId);

        if (user is null)
        {
            return Result<ICollection<User>>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var followers = user.Followers.Select(f => f.Follower).ToList();
            
        return Result<ICollection<User>>.Success(followers);
    }
}