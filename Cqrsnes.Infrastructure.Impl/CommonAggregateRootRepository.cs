using System;

namespace Cqrsnes.Infrastructure.Impl
{
    public class CommonAggregateRootRepository : IAggregateRootRepository
    {
        private readonly IEventStore store;
        private readonly IBus bus;

        public CommonAggregateRootRepository(IEventStore store, IBus bus)
        {
            this.store = store;
            this.bus = bus;
        }

        public void Save<T>(T instance) where T : AggregateRoot, new()
        {
            var events = instance.GetUncommittedChanges();
            store.SaveEvents(instance.Id, events);

            foreach (var @event in events)
            {
                bus.Publish(@event);
            }

            instance.MarkChangesAsCommitted();
        }

        public T GetById<T>(Guid id) where T : AggregateRoot, new()
        {
            var instance = new T();

            var events = store.GetEventsForAggregate(id);
            instance.LoadFromHistory(events);

            return instance;
        }
    }
}