using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.IsPostLiked;

public class IsPostLikedQueryHandler : IQueryHandler<IsPostLikedQuery, bool>
{
    private readonly PostDbContext _context;
    private readonly ILogger<IsPostLikedQueryHandler> _logger;

    public IsPostLikedQueryHandler(PostDbContext context, ILogger<IsPostLikedQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<bool, Error>> HandleAsync(IsPostLikedQuery query)
    {
        if (query.UserId == null)
        {
            _logger.LogWarning("UserId is null in IsPostLikedQuery for PostId: {PostId}", query.PostId);
            return Result<bool>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == query.PostId);

        if (post == null)
        {
            _logger.LogWarning("Attempted to check if a post is liked that does not exist: PostId: {PostId}", query.PostId);
            return Result<bool>.Failure(new Error(ResponseMessages.PostNotFound));
        }
        
        var isAlreadyLiked = await _context.Likes
            .AsNoTracking()
            .AnyAsync(l => l.PostId == query.PostId && l.UserId == query.UserId);

        return Result<bool>.Success(isAlreadyLiked);
    }
}