using Cqrsnes.Infrastructure;

namespace Cqrsnes.Test
{
    public class TestBus : IBus
    {
        public void Publish(Event @event)
        {
        }

        public void Send(Command command)
        {
        }
    }
}