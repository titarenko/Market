using System;
using System.Threading;

namespace Cqrsnes.Infrastructure.Impl
{
    /// <summary>
    /// Simple bus that publishes events asynchronously 
    /// and sends commands synchronously within one process.
    /// </summary>
    public class SimpleBus : IBus
    {
        private readonly IDependencyResolver resolver;

        /// <summary>
        /// Constructs new instance.
        /// </summary>
        /// <param name="resolver">Dependency resolver instance.</param>
        public SimpleBus(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Publishes event to multiple or none subscribers (handlers).
        /// </summary>
        /// <param name="event">Event to publish.</param>
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

        /// <summary>
        /// Sends command to exactly one receiver (handler).
        /// </summary>
        /// <param name="command">Command to send.</param>
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