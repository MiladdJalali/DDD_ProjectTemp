using ProjectTemp.Domain.Properties;

namespace ProjectTemp.Domain.Aggregates.Users.Rules;

public class UserUsernameCannotBeEmptyRule : IBusinessRule
{
    private readonly string value;

    internal UserUsernameCannotBeEmptyRule(string value)
    {
        this.value = value;
    }

    public string Message { get; } = DomainResources.User_UsernameCannotBeEmpty;

    public string Details { get; } = string.Empty;

    public bool IsBroken()
    {
        return string.IsNullOrWhiteSpace(value);
    }
}