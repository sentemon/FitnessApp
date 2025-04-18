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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.UserId);
        if (user is null)
        {
            return Result<User>.Failure(new Error("User not found."));
        }
        
        return Result<User>.Success(user);
    }
}