using System;
using System.Linq;

namespace ProjectTemp.Domain.UnitTest.Helpers
{
    public static class Extensions
    {
        public static T AssertPublishedDomainEvent<T>(this Entity entity)
            where T : IDomainEvent
        {
            var domainEvent = entity.DomainEvents.OfType<T>().SingleOrDefault();
            if (domainEvent is null)
                throw new Exception($"{typeof(T).Name} event not published");

            return domainEvent;
        }
    }
}