using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Handlers;
using Ninject.Modules;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    /// <summary>
    /// Binds command handlers.
    /// </summary>
    public class CommandHandlersNinjectModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Route<CreateUser, UserCommandHandler>();
            Route<LogIn, UserCommandHandler>();
            Route<LogOut, UserCommandHandler>();
            Route<GiveMoney, UserCommandHandler>();

            Route<CreateStore, StoreCommandHandler>();

            Route<CreateArticle, ArticleCommandHandler>();

            Route<CreateOffer, OfferCommandHandler>();

            Route<BuyArticle, PurchaseSaga>();
        }

        private void Route<TCommand, THandler>()
            where TCommand : Command
            where THandler : ICommandHandler<TCommand>
        {
            Bind<ICommandHandler<TCommand>>()
                .To<THandler>()
                .InThreadScope();
        }
    }
}
