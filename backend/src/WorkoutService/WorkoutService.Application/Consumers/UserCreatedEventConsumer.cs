using MassTransit;
using Shared.DTO.Messages;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Consumers;

public class UserCreatedEventConsumer : IConsumer<UserCreatedEventMessage>
{
    private readonly WorkoutDbContext _dbContext;

    public UserCreatedEventConsumer(WorkoutDbContext dbContext)
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