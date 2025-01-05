using MassTransit;
using Microsoft.EntityFrameworkCore;
using PostService.Persistence;
using Shared.DTO;

namespace PostService.Application.Consumers;

public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
{
    private readonly PostDbContext _dbContext;

    public UserUpdatedEventConsumer(PostDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        var @event = context.Message;

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.Id);

        user?.Update(
            @event.FirstName,
            @event.LastName,
            @event.Username,
            @event.ImageUrl
        );

        await _dbContext.SaveChangesAsync();
    }
}