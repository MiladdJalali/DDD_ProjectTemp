using ProjectTemp.Domain.Exceptions;

namespace ProjectTemp.Domain;

public abstract class ValueObject
{
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        var valueObject = (ValueObject) obj;

        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    protected static void CheckRule(IBusinessRule rule)
    {
        if (!rule.IsBroken())
            return;

        throw new DomainException(rule.Message, rule.Details);
    }

    protected abstract IEnumerable<object?> GetAtomicValues();
}