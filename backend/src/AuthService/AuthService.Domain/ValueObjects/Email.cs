namespace AuthService.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(value));
        }

        if (!value.Contains('@') || !value.Contains('.'))
        {
            throw new ArgumentException("Email is not in a valid format.", nameof(value));
        }

        Value = value;
    }

    public static Email Create(string value) => new (value);

    public override string ToString() => Value;
}