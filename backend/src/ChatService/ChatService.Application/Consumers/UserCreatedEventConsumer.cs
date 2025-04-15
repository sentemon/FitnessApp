using ChatService.Domain.Entities;
using ChatService.Persistence;
using MassTransit;
using Shared.DTO.Messages;

namespace ChatService.Application.Consumers;

public class UserCreatedEventConsumer : IConsumer<UserCreatedEventMessage>
{
    private readonly ChatDbContext _dbContext;

    public UserCreatedEventConsumer(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserCreatedEventMessage> context)
    {
        var @event = context.Message;

        var user = User.Create(
            @event.Id,
            @event.FirstName,
            @event.LastName,
            @event.Username,
            @event.ImageUrl
        );

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}