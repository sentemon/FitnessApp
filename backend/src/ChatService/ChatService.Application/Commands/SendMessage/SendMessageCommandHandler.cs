using ChatService.Domain.Constants;
using ChatService.Domain.Entities;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace ChatService.Application.Commands.SendMessage;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, Message>
{
    private readonly ChatDbContext _context;
    private readonly ILogger<SendMessageCommandHandler> _logger;

    public SendMessageCommandHandler(ChatDbContext context, ILogger<SendMessageCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<Message, Error>> HandleAsync(SendMessageCommand command)
    {
        var sender = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.ReceiverId);
        if (sender is null || receiver is null)
        {
            _logger.LogWarning("Attempted to send a message with non-existent users: SenderId: {SenderId}, ReceiverId: {ReceiverId}", command.UserId, command.ReceiverId);
            return Result<Message>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var chat = await _context.Chats
            .Include(c => c.UserChats)
            .FirstOrDefaultAsync(c =>
                c.UserChats.Any(uc => uc.UserId == sender.Id) &&
                c.UserChats.Any(uc => uc.UserId == receiver.Id));
        
        if (chat is null)
        {
            chat = Chat.Create(sender.Id, receiver.Id);
            
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        var message = Message.Create(sender.Id, chat.Id, command.Content.Trim());
        _context.Messages.Add(message);

        await _context.SaveChangesAsync();

        return Result<Message>.Success(message);
    }
}