using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Commands.CreateChat;

public class CreateChatCommandHandler : ICommandHandler<CreateChatCommand, Chat>
{
    private readonly ChatDbContext _context;

    public CreateChatCommandHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<Chat, Error>> HandleAsync(CreateChatCommand command)
    {
        if (string.IsNullOrEmpty(command.UserId1) || string.IsNullOrEmpty(command.UserId2))
        {
            return Result<Chat>.Failure(new Error("User Id cannot be empty."));
        }
        
        var exists = await _context.Chats.Include(c => c.UserChats)
            .AnyAsync(c => 
                c.UserChats.Any(uc => uc.UserId == command.UserId1) && 
                c.UserChats.Any(uc => uc.UserId == command.UserId2)
            );

        if (exists)
        {
            return Result<Chat>.Failure(new Error("Chat between users already exists."));
        }
        
        var chat = Chat.Create(command.UserId1, command.UserId2);
        _context.Chats.Add(chat);

        await _context.SaveChangesAsync();

        return Result<Chat>.Success(chat);
    }
}