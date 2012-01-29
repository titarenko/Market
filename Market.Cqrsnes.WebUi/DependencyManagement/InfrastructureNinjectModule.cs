using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Cqrsnes.Infrastructure.Impl;
using Cqrsnes.Test;
using Market.Cqrsnes.Domain.Utility;
using Market.Cqrsnes.Projection.Models;
using Market.Cqrsnes.WebUi.Infrastructure;
using Ninject;
using Ninject.Modules;
using Raven.Client;
using Raven.Client.Document;
using ServiceStack.Redis;
using Ninject.Extensions.Conventions;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    /// <summary>
    /// Binds infrastructure services.
    /// </summary>
    public class InfrastructureNinjectModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IDependencyResolver>()
                .To<NinjectDependencyResolver>()
                .InSingletonScope();

            Bind<System.Web.Mvc.IDependencyResolver>()
                .To<NinjectDependencyResolver>()
                .InSingletonScope();

            Bind<IBus>()
                .To<SimpleBus>()
                .InSingletonScope();

            Bind<IAggregateRootRepository>()
                .To<CommonAggregateRootRepository>()
                .InRequestScope();

            Bind<IEventStore>()
                .To<RavenEventStore>()
                .InRequestScope();

            Bind<IDocumentSession>()
                .ToMethod(x => x.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation(x => x.Dispose());

            Bind<IDocumentStore>()
                .ToMethod(context =>
                              {
                                  var store = new DocumentStore { ConnectionStringName = "eventStore" };
                                  store.Initialize();
                                  return store;
                              })
                .InSingletonScope();

            Bind<IRepository>()
                .To<RedisRepository>()
                .InSingletonScope();

            Bind<IRedisClient>()
                .To<RedisClient>()
                .InSingletonScope();

            Bind<IPasswordHashGenerator>()
                .To<PasswordHashGenerator>()
                .InSingletonScope();

            Bind<ISystemContext>()
                .To<WebSystemContext>()
                .InRequestScope();

            Kernel.Scan(x =>
                            {
                                x.FromAssembliesMatching("*");
                                x.WhereTypeInheritsFrom<ISpecificationHolder>();
                                x.BindWith<BindToImplementedInterfacesBindingGenerator>();
                            });

            Bind<IEnumerable<ISpecificationHolder>>()
                .ToMethod(x => x.Kernel.GetAll<ISpecificationHolder>())
                .InSingletonScope();
        }
    }
}
