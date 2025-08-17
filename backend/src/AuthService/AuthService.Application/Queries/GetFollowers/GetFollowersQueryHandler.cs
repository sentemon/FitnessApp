using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetFollowers;

public class GetFollowersQueryHandler : IQueryHandler<GetFollowersQuery, ICollection<User>>
{
    private readonly AuthDbContext _context;
    private readonly ILogger<GetFollowersQueryHandler> _logger;

    public GetFollowersQueryHandler(AuthDbContext context, ILogger<GetFollowersQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<ICollection<User>, Error>> HandleAsync(GetFollowersQuery query)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Username)
            .Include(u => u.Followers)
                .ThenInclude(f => f.Follower)
            .FirstOrDefaultAsync(u => u.Id == query.UserId);

        if (user is null)
        {
            _logger.LogWarning("Get followers attempt with non-existing user ID: {UserId}", query.UserId);
            return Result<ICollection<User>>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var followers = user.Followers.Select(f => f.Follower).ToList();

        return Result<ICollection<User>>.Success(followers);
    }
}