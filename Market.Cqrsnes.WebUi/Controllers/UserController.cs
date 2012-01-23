using System;
using System.Linq;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Controllers
{
    /// <summary>
    /// Controller for managing of users.
    /// </summary>
    public class UserController : Controller
    {
        private readonly IBus bus;
        private readonly IRepository repository;
        private readonly ISystemContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        /// <param name="repository">The repository.</param>
        /// <param name="context">The system context.</param>
        public UserController(IBus bus, IRepository repository, ISystemContext context)
        {
            this.bus = bus;
            this.repository = repository;
            this.context = context;
        }
        
        /// <summary>
        /// Renders registration page.
        /// </summary>
        /// <returns>
        /// Registration page.
        /// </returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Registers new user and logs him/her into the system.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// Redirect to home page.
        /// </returns>
        [HttpPost]
        public ActionResult Register(string name, string password)
        {
            var id = Guid.NewGuid();

            bus.Send(new CreateUser
                {
                    Id = id,
                    Name = name,
                    Password = password
                });

            bus.Send(new LogIn
                {
                    UserId = id,
                    Password = password
                });

            return RedirectToHomePage();
        }

        /// <summary>
        /// Renders log in page.
        /// </summary>
        /// <returns>
        /// Log in page.
        /// </returns>
        public ActionResult LogIn(string returnUrl)
        {
            return View(returnUrl as object);
        }

        /// <summary>
        /// Logs user into the system.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>
        /// Redirect to home page.
        /// </returns>
        [HttpPost]
        public ActionResult LogIn(string name, string password, string returnUrl)
        {
            bus.Send(new LogIn
                {
                    UserId = repository
                        .GetAll<User>()
                        .Single(x => x.Name == name).Id,
                    Password = password
                });

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToHomePage();
            }
            
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Logs user out of system.
        /// </summary>
        /// <returns>
        /// Redirect to home page.
        /// </returns>
        [HttpPost]
        public ActionResult LogOut()
        {
            if (context.Principal.Identity.IsAuthenticated)
            {
                bus.Send(new LogOut
                    {
                        UserId = context.User.Id
                    });
            }

            return RedirectToHomePage();
        }

        /// <summary>
        /// Renders list of users.
        /// </summary>
        /// <returns>
        /// List of users.
        /// </returns>
        public ActionResult List()
        {
            return View(repository.GetAll<User>());
        }

        private RedirectToRouteResult RedirectToHomePage()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
