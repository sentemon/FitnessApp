using MassTransit;
using PostService.Domain.Entities;
using PostService.Persistence;
using Shared.DTO;

namespace PostService.Application.Events;

public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
{
    private readonly PostDbContext _dbContext;

    public UserCreatedEventConsumer(PostDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
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