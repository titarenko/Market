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
    /// <summary>
    /// Represents web application.
    /// </summary>
    public class MvcApplication : NinjectHttpApplication
    {
        private const string ERROR_ROUTE = "Error";

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcApplication"/> class.
        /// </summary>
        public MvcApplication()
        {
            BeginRequest += OnBeginRequest;
            EndRequest += EndRequestHandler;
            Error += OnError;
        }

        /// <summary>
        /// Called when the application is started.
        /// </summary>
        protected override void OnApplicationStarted()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>
        /// The created kernel.
        /// </returns>
        protected override IKernel CreateKernel()
        {
            return new StandardKernel(
                new InfrastructureNinjectModule(),
                new CommandHandlersNinjectModule(),
                new EventHandlersNinjectModule());
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

            application.Context.Response.Redirect("~/" + ERROR_ROUTE);
        }

        private void EndRequestHandler(object sender, EventArgs e)
        {
            Kernel.Get<RavenSessionManager>().StopSession();
        }

        private void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Error",
                ERROR_ROUTE,
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
                "Log",
                "Log",
                new
                {
                    controller = "Home",
                    action = "Log",
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
    }
}
