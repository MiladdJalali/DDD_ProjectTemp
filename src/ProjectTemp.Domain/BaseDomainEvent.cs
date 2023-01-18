namespace ProjectTemp.Domain
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        protected BaseDomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            EventTime = DateTimeOffset.UtcNow;
        }

        public Guid AggregateId { get; }

        public DateTimeOffset EventTime { get; }

        public virtual Dictionary<string, object?> Flatten()
        {
            return GetType()
                .GetProperties()
                .Where(i => i.Name != nameof(AggregateId) && i.Name != nameof(EventTime))
                .ToDictionary(i => i.Name, i => i.GetValue(this));
        }
    }
}
