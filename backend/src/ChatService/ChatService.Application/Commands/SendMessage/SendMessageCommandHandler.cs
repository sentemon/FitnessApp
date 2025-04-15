using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Commands.SendMessage;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, Message>
{
    private readonly ChatDbContext _context;

    public SendMessageCommandHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<Message, Error>> HandleAsync(SendMessageCommand command)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == command.ChatId);
        if (chat is null)
        {
            return Result<Message>.Failure(new Error("Chat not found."));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        if (user is null)
        {
            return Result<Message>.Failure(new Error("User not found."));
        }

        var message = Message.Create(user.Id, chat.Id, command.Content.Trim());
        _context.Messages.Add(message);

        await _context.SaveChangesAsync();

        return Result<Message>.Success(message);
    }
}