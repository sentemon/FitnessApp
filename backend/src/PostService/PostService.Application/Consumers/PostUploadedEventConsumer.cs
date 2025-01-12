using MassTransit;
using Microsoft.EntityFrameworkCore;
using PostService.Persistence;
using Shared.DTO.Messages;

namespace PostService.Application.Consumers;

public class PostUploadedEventConsumer : IConsumer<PostUploadedEventMessage>
{
    private readonly PostDbContext _dbContext;

    public PostUploadedEventConsumer(PostDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<PostUploadedEventMessage> context)
    {
        var @event = context.Message;

        var post = await _dbContext.Posts.FirstAsync(p => p.Id == @event.PostId);
        
        post.SetContentUrl(@event.ContentUrl);
        await _dbContext.SaveChangesAsync();
    }
}