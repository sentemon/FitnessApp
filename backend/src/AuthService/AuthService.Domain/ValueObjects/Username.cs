namespace AuthService.Domain.ValueObjects;

public class Username
{
    public string Value { get; }

    private Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(value));
        }

        if (value.Length < 3 || value.Length > 20)
        {
            throw new ArgumentException("Username must be between 3 and 20 characters.", nameof(value));
        }

        Value = value;
    }

    public static Username Create(string value) => new (value.ToLower());

    public override string ToString() => Value;
}