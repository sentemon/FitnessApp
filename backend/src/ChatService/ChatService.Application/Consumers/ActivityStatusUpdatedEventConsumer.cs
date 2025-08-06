using ChatService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Messages;

namespace ChatService.Application.Consumers;

public class ActivityStatusUpdatedEventConsumer : IConsumer<ActivityStatusUpdatedEventMessage>
{
    private readonly ChatDbContext _dbContext;

    public ActivityStatusUpdatedEventConsumer(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ActivityStatusUpdatedEventMessage> context)
    {
        var @event = context.Message;
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.UserId);
        if (user is null)
        {
            return;
        }
        
        user.UpdateActivityStatus(@event.LastSeenAt);

        await _dbContext.SaveChangesAsync();
    }
}