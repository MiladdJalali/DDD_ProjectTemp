using ProjectTemp.Domain.Aggregates.Users.ValueObjects;

namespace ProjectTemp.Domain.Aggregates.Users.Events;

public class UserPasswordChangedEvent : BaseDomainEvent
{
    public UserPasswordChangedEvent(UserId userId)
        : base(userId.Value)
    {
    }
}