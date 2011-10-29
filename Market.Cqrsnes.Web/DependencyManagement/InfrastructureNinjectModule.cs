using Cqrsnes.Infrastructure;
using Cqrsnes.Infrastructure.Impl;
using Market.Cqrsnes.Projection;
using Ninject;
using Ninject.Modules;
using Raven.Client.Document;
using ServiceStack.Redis;

namespace Market.Cqrsnes.Web.DependencyManagement
{
    public class InfrastructureNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDependencyResolver>()
                .To<NinjectDependencyResolver>()
                .InSingletonScope();

            Bind<IBus>()
                .To<Bus>()
                .InSingletonScope();

            Bind<IAggregateRootRepository>()
                .To<CommonAggregateRootRepository>()
                .InSingletonScope();

            Bind<IEventStore>()
                .To<RavenEventStore>()
                .InSingletonScope()
                .WithConstructorArgument(
                    "session",
                    context => context.Kernel.Get<RavenSessionManager>().Session);

            Bind<RavenSessionManager>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument(
                    "store", context =>
                                 {
                                     var store = new DocumentStore {ConnectionStringName = "eventStore"};
                                     store.Initialize();
                                     return store;
                                 });

            Bind<IRepository>()
                .To<RedisRepository>()
                .InSingletonScope()
                .WithConstructorArgument("client", new RedisClient());

            Bind<ArticleViewModelManager>()
                .ToSelf()
                .InSingletonScope();
        }
    }
}