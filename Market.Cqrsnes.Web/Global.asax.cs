using System.Web.Mvc;
using System.Web.Routing;
using Market.Cqrsnes.Web.DependencyManagement;
using Ninject;
using Ninject.Web.Mvc;

namespace Market.Cqrsnes.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
        public MvcApplication()
        {
            EndRequest += EndRequestHandler;
        }

        void EndRequestHandler(object sender, System.EventArgs e)
        {
            Kernel.Get<RavenSessionManager>().StopSession();
        }

        static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                    {
                        controller = "Article",
                        action = "List",
                        id = UrlParameter.Optional
                    },
                new
                    {
                        controller = @"[^\.]*"
                    });
        }

        protected override void OnApplicationStarted()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected override IKernel CreateKernel()
        {
            return new StandardKernel(
                new InfrastructureNinjectModule(),
                new CommandHandlersNinjectModule(),
                new EventHandlersNinjectModule());
        }
    }
}