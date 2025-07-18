using Microsoft.EntityFrameworkCore;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Queries.IsPostLiked;

public class IsPostLikedQueryHandler : IQueryHandler<IsPostLikedQuery, bool>
{
    private readonly PostDbContext _context;

    public IsPostLikedQueryHandler(PostDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<bool, Error>> HandleAsync(IsPostLikedQuery query)
    {
        if (query.UserId == null)
        {
            return Result<bool>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == query.PostId);

        if (post == null)
        {
            return Result<bool>.Failure(new Error(ResponseMessages.PostNotFound));
        }
        
        var isAlreadyLiked = await _context.Likes
            .AsNoTracking()
            .AnyAsync(l => l.PostId == query.PostId && l.UserId == query.UserId);

        return Result<bool>.Success(isAlreadyLiked);
    }
}