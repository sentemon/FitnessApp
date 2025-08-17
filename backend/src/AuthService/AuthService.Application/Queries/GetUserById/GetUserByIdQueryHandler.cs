using AuthService.Domain.Constants;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace AuthService.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(IUserService userService, ILogger<GetUserByIdQueryHandler> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IResult<User, Error>> HandleAsync(GetUserByIdQuery query)
    {
        if (query.Id == null)
        {
            _logger.LogWarning("Get user by ID attempt with null UserId.");
            return Result<User>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        var user = await _userService.GetByIdAsync(query.Id);

        if (user == null)
        {
            _logger.LogWarning("Get user by ID attempt with non-existing user ID: {UserId}", query.Id);
            return Result<User>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        return Result<User>.Success(user);
    }
}