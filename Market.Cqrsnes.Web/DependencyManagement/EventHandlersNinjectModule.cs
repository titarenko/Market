using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain;
using Market.Cqrsnes.Projection;
using Ninject.Modules;

namespace Market.Cqrsnes.Web.DependencyManagement
{
    public class EventHandlersNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Route<ArticleCreated, ArticleViewModelManager>();
            Route<ArticleDelivered, ArticleViewModelManager>();
            Route<ArticleBought, ArticleViewModelManager>();
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