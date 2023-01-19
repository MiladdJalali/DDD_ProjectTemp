using ProjectTemp.Domain.Aggregates.Users.ValueObjects;

namespace ProjectTemp.Domain.Aggregates.Users.Events;

public class UserDeletedEvent : BaseDomainEvent
{
    public UserDeletedEvent(UserId userId)
        : base(userId.Value)
    {
    }
}