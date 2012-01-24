using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection;
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

            Route<StoreCreated, StoreEventHandler>();

            Route<ArticleCreated, ArticleEventHandler>();

            Route<OfferCreated, OfferEventHandler>();
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
