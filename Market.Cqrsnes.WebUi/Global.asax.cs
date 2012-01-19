using System;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using log4net.Config;
using Market.Cqrsnes.Projection;
using Market.Cqrsnes.WebUi.DependencyManagement;
using Ninject;
using Ninject.Web.Mvc;

[assembly: XmlConfigurator(Watch = true)]

namespace Market.Cqrsnes.WebUi
{
    public class MvcApplication : NinjectHttpApplication
    {
        public MvcApplication()
        {
            BeginRequest += OnBeginRequest;
            EndRequest += EndRequestHandler;
            Error += OnError;
        }

        private void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            Context.User = Kernel.Get<ISystemContext>().Principal;
        }

        private void OnError(object sender, EventArgs eventArgs)
        {
            var application = sender as NinjectHttpApplication;
            if (application == null)
            {
                return;
            }

            var error = application.Context.Server.GetLastError();
            LogManager.GetLogger(typeof(MvcApplication)).Error("Application level error.", error);
            application.Context.Server.ClearError();

            application.Context.Response.Redirect("/Error");
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
                "Error",
                "Error",
                new
                    {
                        controller = "Home",
                        action = "Error",
                        id = UrlParameter.Optional
                    },
                new
                    {
                        controller = @"[^\.]*"
                    });

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                    {
                        controller = "Home",
                        action = "Index",
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
            XmlConfigurator.Configure();
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
