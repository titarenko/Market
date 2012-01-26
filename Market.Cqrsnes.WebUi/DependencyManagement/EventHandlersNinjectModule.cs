using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Handlers;
using Market.Cqrsnes.Projection.Handlers;
using Ninject.Modules;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    /// <summary>
    /// Binds event handlers.
    /// </summary>
    public class EventHandlersNinjectModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Route<UserCreated, UserEventHandler>();
            Route<UserLoggedIn, UserEventHandler>();
            Route<UserLoggedOut, UserEventHandler>();
            Route<BalanceIncreased, UserEventHandler>();

            Route<StoreCreated, StoreEventHandler>();

            Route<ArticleCreated, ArticleEventHandler>();

            Route<OfferCreated, OfferEventHandler>();

            Route<MoneyReserved, PurchaseSaga>();
            Route<ArticleReserved, PurchaseSaga>();
            Route<BalanceDecreased, PurchaseSaga>();
            Route<BalanceDecreaseFailed, PurchaseSaga>();

            Route<PurchaseCreated, PurchaseEventHandler>();
        }

        private void Route<TEvent, THandler>() 
            where TEvent : Event 
            where THandler : IEventHandler<TEvent>
        {
            Bind<IEventHandler<TEvent>>()
                .To<THandler>()
                .InThreadScope();
        }
    }
}
