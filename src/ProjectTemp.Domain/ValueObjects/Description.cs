namespace ProjectTemp.Domain.ValueObjects;

public class Description : ValueObject
{
    private Description()
    {
    }

    public string? Value { get; private init; }

    public static Description? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return new Description {Value = value.Trim()};
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}