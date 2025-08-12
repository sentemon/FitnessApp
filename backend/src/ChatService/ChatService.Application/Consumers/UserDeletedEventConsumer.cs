using ChatService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Messages;

namespace ChatService.Application.Consumers;

public class UserDeletedEventConsumer : IConsumer<UserDeletedEventMessage>
{
    private readonly ChatDbContext _dbContext;

    public UserDeletedEventConsumer(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserDeletedEventMessage> context)
    {
        var @event = context.Message;
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.UserId);

        if (user is null)
        {
            return;
        }
        
        _dbContext.Users.Remove(user);
        
        await _dbContext.SaveChangesAsync();
    }
}