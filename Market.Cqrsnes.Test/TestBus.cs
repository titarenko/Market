using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Test
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