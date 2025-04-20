namespace ChatService.Domain.Entities;

public class UserChat
{
    public string UserId { get; private set; }

    public User User { get; private set; }

    public Guid ChatId { get; private set; }

    public Chat Chat { get; private set; }

    internal UserChat(string userId, Guid chatId)
    {
        UserId = userId;
        ChatId = chatId;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private UserChat() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}