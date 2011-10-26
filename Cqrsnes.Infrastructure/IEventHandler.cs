namespace Cqrsnes.Infrastructure
{
    public interface IEventHandler<in T> where T : Event
    {
        void Handle(T @event);
    }
}