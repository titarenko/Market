using System;
using System.Threading;

namespace Cqrsnes.Infrastructure.Impl
{
    public class Bus : IBus
    {
        private readonly IDependencyResolver resolver;

        public Bus(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Publish(Event @event)
        {
            var type = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            foreach (var handler in resolver.ResolveMultiple(type))
            {
                var instance = handler;
                ThreadPool.QueueUserWorkItem(
                    x => instance.GetType()
                             .GetMethod("Handle", new[] {@event.GetType()})
                             .Invoke(instance, new object[] {@event}));
            }
        }

        public void Send(Command command)
        {
            var type = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = resolver.Resolve(type);
            if (handler == null)
            {
                throw new InvalidOperationException("Can't find handler for given command.");
            }

            handler.GetType()
                .GetMethod("Handle", new[] {command.GetType()})
                .Invoke(handler, new object[] {command});
        }
    }
}