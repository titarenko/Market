using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Raven.Abstractions.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace Cqrsnes.Infrastructure.Impl
{
    public class RavenEventStore : IEventStore
    {
        private readonly IDocumentSession session;
        
        public RavenEventStore(IDocumentSession session)
        {
            this.session = session;
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                session.Store(new EventDescriptor
                                  {
                                      AggregateId = aggregateId,
                                      EventData = @event
                                  });
            }
            session.SaveChanges();
        }

        public IEnumerable<Event> GetEventsForAggregate(Guid id)
        {
            var objects = session.Query<EventDescriptor>()
                .Where(x => x.AggregateId == id)
                .Select(x => x.EventData)
                .ToArray()
                .Cast<DynamicJsonObject>();

            foreach (var json in objects)
            {
                var o = (DynamicJsonObject) json.GetValue("EventData");
                var typeName = o.GetValue("$type") as string;
                if (typeName == null)
                {
                    throw new InvalidOperationException("Can't read event type name.");
                }

                var type = Type.GetType(typeName);
                if (!typeof (Event).IsAssignableFrom(type))
                {
                    throw new InvalidOperationException("Event doesn't inherit Event abstract class.");
                }

                var instance = Activator.CreateInstance(type);

                foreach (KeyValuePair<string, object> property in o)
                {
                    var propertyInfo = type.GetProperty(property.Key);
                    var value = TypeDescriptor.GetConverter(propertyInfo.PropertyType)
                        .ConvertFromInvariantString(property.Value.ToString());
                    propertyInfo.SetValue(instance, value, null);
                }

                yield return instance as Event;
            }
        }

        public class EventDescriptor
        {
            public Guid AggregateId { get; set; }

            public object EventData { get; set; }
        }
    }
}