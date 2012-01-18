using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Handlers;
using Ninject.Modules;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    public class CommandHandlersNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Route<CreateArticle, ArticleCommandHandler>();
            Route<DeliverArticle, ArticleCommandHandler>();
            Route<BuyArticle, ArticleCommandHandler>();

            Route<CreateUser, UserCommandHandler>();
            Route<LogIn, UserCommandHandler>();
            Route<LogOut, UserCommandHandler>();
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
