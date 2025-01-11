using MassTransit;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.DTO;
using Shared.DTO.Messages;

namespace PostService.Application.Consumers;

public class UserCreatedEventConsumer : IConsumer<UserCreatedEventMessage>
{
    private readonly PostDbContext _dbContext;

    public UserCreatedEventConsumer(PostDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserCreatedEventMessage> context)
    {
        var @event = context.Message;

        var user = new User(
            @event.Id,
            @event.FirstName,
            @event.LastName,
            @event.Username,
            @event.ImageUrl,
            @event.CreatedAt
        );

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}