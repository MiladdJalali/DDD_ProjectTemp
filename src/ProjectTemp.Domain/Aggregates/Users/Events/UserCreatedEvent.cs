using ProjectTemp.Domain.Aggregates.Users.ValueObjects;

namespace ProjectTemp.Domain.Aggregates.Users.Events
{
    public class UserCreatedEvent : BaseDomainEvent
    {
        public UserCreatedEvent(UserId userId)
            : base(userId.Value)
        {
        }
    }
}