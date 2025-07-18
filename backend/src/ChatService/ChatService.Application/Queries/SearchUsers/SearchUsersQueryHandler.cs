using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Queries.SearchUsers;

public class SearchUsersQueryHandler : IQueryHandler<SearchUsersQuery, IEnumerable<User>>
{
    private readonly ChatDbContext _context;

    public SearchUsersQueryHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<IEnumerable<User>, Error>> HandleAsync(SearchUsersQuery query)
    {
        var search = $"%{query.Search}%";

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id != query.UserId)   
            .Where(u =>
                EF.Functions.Like(u.Username, search) ||
                EF.Functions.Like(u.FirstName + " " + u.LastName, search))
            .OrderBy(u => u.Username)
            .Take(10)
            .ToListAsync();

        return Result<IEnumerable<User>>.Success(users);
    }
}