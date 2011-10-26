using System;
using System.Collections.Generic;

namespace Cqrsnes.Infrastructure
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<Event> events);
        IEnumerable<Event> GetEventsForAggregate(Guid id);
    }
}