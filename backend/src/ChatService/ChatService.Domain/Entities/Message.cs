namespace ChatService.Domain.Entities;

public class Message
{
    public Guid Id { get; private set; }
    public string SenderId { get; private set; }
    public User Sender { get; private set; }
    
    public Guid ChatId { get; private set; }
    public Chat Chat { get; private set; }
    
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsRead { get; private set; }

    private Message(string senderId, Guid chatId, string content, DateTime sentAt)
    {
        SenderId = senderId;
        ChatId = chatId;
        Content = content;
        SentAt = sentAt;
    }

    public static Message Create(string senderId, Guid chatId, string content, DateTime sentAt)
    {
        return new Message(senderId, chatId, content, sentAt);
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Message() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}