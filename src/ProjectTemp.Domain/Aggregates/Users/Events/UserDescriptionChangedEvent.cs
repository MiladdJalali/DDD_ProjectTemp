using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Domain.ValueObjects;

namespace ProjectTemp.Domain.Aggregates.Users.Events
{
    public class UserDescriptionChangedEvent : BaseDomainEvent
    {
        public UserDescriptionChangedEvent(UserId userId, Description? oldValue, Description? newValue)
            : base(userId.Value)
        {
            OldValue = oldValue?.Value;
            NewValue = newValue?.Value;
        }

        public string? OldValue { get; }

        public string? NewValue { get; }
    }
}