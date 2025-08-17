using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Commands.CreateChat;

public class CreateChatCommandHandler : ICommandHandler<CreateChatCommand, Chat>
{
    private readonly ChatDbContext _context;
    private readonly ILogger<CreateChatCommandHandler> _logger;

    public CreateChatCommandHandler(ChatDbContext context, ILogger<CreateChatCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<Chat, Error>> HandleAsync(CreateChatCommand command)
    {
        if (string.IsNullOrEmpty(command.UserId1) || string.IsNullOrEmpty(command.UserId2))
        {
            _logger.LogWarning("Attempted to create a chat with empty user IDs: UserId1: {UserId1}, UserId2: {UserId2}", command.UserId1, command.UserId2);
            return Result<Chat>.Failure(new Error(ResponseMessages.UserIdCannotBeEmpty));
        }
        
        var exists = await _context.Chats.Include(c => c.UserChats)
            .AnyAsync(c => 
                c.UserChats.Any(uc => uc.UserId == command.UserId1) && 
                c.UserChats.Any(uc => uc.UserId == command.UserId2)
            );

        if (exists)
        {
            _logger.LogWarning("Chat between users {UserId1} and {UserId2} already exists.", command.UserId1, command.UserId2);
            return Result<Chat>.Failure(new Error(ResponseMessages.ChatBetweenUsersAlreadyExists));
        }
        
        var chat = Chat.Create(command.UserId1, command.UserId2);
        _context.Chats.Add(chat);

        await _context.SaveChangesAsync();

        return Result<Chat>.Success(chat);
    }
}