using System.Web.Mvc;
using System.Web.Routing;
using Market.Cqrsnes.WebUi.DependencyManagement;
using Ninject;
using Ninject.Web.Mvc;

namespace Market.Cqrsnes.WebUi
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

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
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
            RegisterGlobalFilters(GlobalFilters.Filters);
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