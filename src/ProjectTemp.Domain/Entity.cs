using ProjectTemp.Domain.Exceptions;

namespace ProjectTemp.Domain
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> domainEvents;

        private bool canBeDeleted;

        protected Entity()
        {
            domainEvents = new List<IDomainEvent>();
        }

        public IEnumerable<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

        public bool CanBeDeleted()
        {
            return canBeDeleted;
        }

        public void ClearEvents()
        {
            domainEvents.Clear();
        }

        protected void MarkAsDeleted()
        {
            canBeDeleted = true;
        }

        protected void AddEvent(IDomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (!rule.IsBroken())
                return;

            throw new DomainException(rule.Message, rule.Details);
        }
    }
}
