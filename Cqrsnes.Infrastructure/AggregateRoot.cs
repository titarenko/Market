using System;
using System.Collections.Generic;

namespace Cqrsnes.Infrastructure
{
    public abstract class AggregateRoot
    {
        protected Guid id;

        private readonly List<Event> uncommittedChanges = new List<Event>();

        protected AggregateRoot()
        {
        }

        protected AggregateRoot(Guid id)
        {
            if (id == default(Guid))
            {
                throw new InvalidOperationException("Can't assign null id.");
            }

            this.id = id;
        }

        public Guid Id
        {
            get
            {
                return id;
            }
        }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return uncommittedChanges;
        }

        public void MarkChangesAsCommitted()
        {
            uncommittedChanges.Clear();
        }

        public void LoadFromHistory(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, true);
            }
        }

        protected void ApplyChange(Event @event, bool isFromHistory = false)
        {
            var type = GetType();
            var eventType = @event.GetType();

            var handlerType = typeof (IChangeAcceptor<>)
                .MakeGenericType(eventType);

            if (handlerType.IsAssignableFrom(type))
            {
                type.GetMethod("Accept", new[] {eventType})
                    .Invoke(this, new object[] {@event});
            }

            if (!isFromHistory)
            {
                uncommittedChanges.Add(@event);
            }
        }
    }
}