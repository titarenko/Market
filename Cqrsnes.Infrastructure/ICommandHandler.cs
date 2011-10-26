namespace Cqrsnes.Infrastructure
{
    public interface ICommandHandler<in T> where T : Command
    {
        void Handle(T command);
    }
}