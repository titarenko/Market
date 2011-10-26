using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain;
using Ninject.Modules;

namespace Market.Cqrsnes.Web.DependencyManagement
{
    public class CommandHandlersNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Route<CreateArticle, ArticleCommandsHandler>();
            Route<DeliverArticle, ArticleCommandsHandler>();
            Route<BuyArticle, ArticleCommandsHandler>();
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