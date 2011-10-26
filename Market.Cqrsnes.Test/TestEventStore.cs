using System;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Test
{
    public class TestEventStore : IEventStore
    {
        private readonly IEnumerable<Event> events;
        private readonly List<Event> produced = new List<Event>();

        public TestEventStore(IEnumerable<Event> events)
        {
            this.events = events;
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events)
        {
            produced.AddRange(events);
        }

        public IEnumerable<Event> GetEventsForAggregate(Guid id)
        {
            return events;
        }

        public IEnumerable<Event> GetProducedEvents()
        {
            return produced;
        }
    }
}