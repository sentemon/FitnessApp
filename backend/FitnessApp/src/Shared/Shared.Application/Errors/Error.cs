namespace Shared.Application.Errors;

public class Error
{
    public string Message { get; }

    public Error(string message)
    {
        Message = message;
    }
}