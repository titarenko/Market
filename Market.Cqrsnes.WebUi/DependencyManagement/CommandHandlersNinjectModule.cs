using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Handlers;
using Market.Cqrsnes.Projection;
using Ninject.Modules;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    public class CommandHandlersNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Route<CreateUser, UserCommandHandler>();
            Route<LogIn, UserCommandHandler>();
            Route<LogOut, UserCommandHandler>();

            Route<CreateStore, StoreCommandHandler>();
        }

        private void Route<TCommand, THandler>()
            where TCommand : Command
            where THandler : ICommandHandler<TCommand>
        {
            Bind<ICommandHandler<TCommand>>()
                .To<THandler>()
                .InSingletonScope();
        }
    }
}
