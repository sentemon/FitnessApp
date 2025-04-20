namespace ChatService.Domain.Entities;

public class Chat
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<UserChat> _userChats = [];
    public IReadOnlyCollection<UserChat> UserChats => _userChats.AsReadOnly();

    private readonly List<Message> _messages = [];
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    public static Chat Create(string userId1, string userId2)
    {
        var chat = new Chat();
        
        chat._userChats.Add(new UserChat(userId1, chat.Id));
        chat._userChats.Add(new UserChat(userId2, chat.Id));

        return chat;
    }
    
    private Chat() { }
}