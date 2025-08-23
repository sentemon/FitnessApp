using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly AuthDbContext _context;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(AuthDbContext context, ILogger<GetUserByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<User, Error>> HandleAsync(GetUserByIdQuery query)
    {
        if (query.Id == null)
        {
            _logger.LogWarning("Get user by ID attempt with null UserId.");
            return Result<User>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == query.Id);

        if (user == null)
        {
            _logger.LogWarning("Get user by ID attempt with non-existing user ID: {UserId}", query.Id);
            return Result<User>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        return Result<User>.Success(user);
    }
}