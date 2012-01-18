using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection;
using Ninject.Modules;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    public class EventHandlersNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Route<ArticleCreated, ArticleViewModelManager>();
            Route<ArticleDelivered, ArticleViewModelManager>();
            Route<ArticleBought, ArticleViewModelManager>();

            Route<UserCreated, UserEventHandler>();
            Route<UserLoggedIn, UserEventHandler>();
            Route<UserLoggedOut, UserEventHandler>();
        }

        private void Route<TEvent, THandler>() 
            where TEvent : Event 
            where THandler : IEventHandler<TEvent>
        {
            Bind<IEventHandler<TEvent>>()
                .To<THandler>()
                .InSingletonScope();
        }
    }
}
