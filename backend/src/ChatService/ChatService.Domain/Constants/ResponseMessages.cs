namespace ChatService.Domain.Constants;

public static class ResponseMessages
{
    public const string UserIdCannotBeEmpty = "User Id cannot be empty.";
    public const string ChatIdCannotBeEmpty = "Chat Id cannot be empty.";
    public const string ChatBetweenUsersAlreadyExists = "Chat between users already exists.";
    public const string ChatNotFound = "Chat not found.";
    public const string UserNotFound = "User not found.";
    public const string NoMessagesInChat = "No messages in the chat";
}