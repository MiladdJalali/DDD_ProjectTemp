using ProjectTemp.Domain.Aggregates.Users.Rules;

namespace ProjectTemp.Domain.Aggregates.Users.ValueObjects;

public class UserUsername : ValueObject
{
    private UserUsername()
    {
    }

    public string? Value { get; private init; }

    public static UserUsername Create(string value)
    {
        CheckRule(new UserUsernameCannotBeEmptyRule(value));

        return new UserUsername {Value = value};
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}