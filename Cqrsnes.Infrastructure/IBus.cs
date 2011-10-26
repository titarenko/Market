namespace Cqrsnes.Infrastructure
{
    public interface IBus
    {
        void Publish(Event @event);
        void Send(Command command);
    }
}