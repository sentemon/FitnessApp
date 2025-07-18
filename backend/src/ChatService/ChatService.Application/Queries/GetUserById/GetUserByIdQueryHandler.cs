using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly ChatDbContext _context;

    public GetUserByIdQueryHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<User, Error>> HandleAsync(GetUserByIdQuery query)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == query.UserId);
        
        if (user is null)
        {
            return Result<User>.Failure(new Error(ResponseMessages.UserNotFound));
        }
        
        return Result<User>.Success(user);
    }
}