using System;
using System.Web;
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
    public class WebApplication : NinjectHttpApplication
    {
        private const string ERROR_ROUTE = "Error";

        private static readonly object syncRoot = new object();
        private static IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplication"/> class.
        /// </summary>
        public WebApplication()
        {
            // force initialization of system context at begin of request
            BeginRequest += (sender, args) => Kernel.Get<ISystemContext>();
            Error += OnError;
        }

        /// <summary>
        /// Gets system context instance.
        /// </summary>
        /// <remarks>
        /// Temporal solution. Will be available until view dependency injection is implemented.
        /// </remarks>
        public static ISystemContext SystemContext
        {
            get
            {
                lock (syncRoot)
                {
                    if (kernel == null)
                    {
                        throw new ApplicationException(
                            "Context of application was accessed before its instantiation.");
                    }

                    return kernel.Get<ISystemContext>();
                }
            }
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
            lock (syncRoot)
            {
                return kernel = new StandardKernel(
                                    new InfrastructureNinjectModule(),
                                    new CommandHandlersNinjectModule(),
                                    new EventHandlersNinjectModule());
            }
        }

        private void OnError(object sender, EventArgs eventArgs)
        {
            var application = sender as NinjectHttpApplication;
            if (application == null)
            {
                return;
            }

            var error = application.Context.Server.GetLastError();
            var message = "Application level error.";
            var logger = LogManager.GetLogger(typeof(WebApplication));
            
            if (error is HttpException)
            {
                logger.Warn(message, error);
            }
            else
            {
                logger.Error(message, error);
            }

            application.Context.Server.ClearError();

            application.Context.Response.Redirect("~/" + ERROR_ROUTE);
        }

        private void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterRoute(routes, ERROR_ROUTE, "Home", "Error");
            RegisterRoute(routes, "Log", "Home", "Log");
            RegisterRoute(routes, "LogIn", "User", "LogIn");
            RegisterRoute(routes, "Register", "User", "Register");
            
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

        private void RegisterRoute(RouteCollection routes, string route, string controller, string action)
        {
            routes.MapRoute(
                route,
                route,
                new
                {
                    controller, 
                    action,
                    id = UrlParameter.Optional
                },
                new
                {
                    controller = @"[^\.]*"
                });
        }
    }
}
