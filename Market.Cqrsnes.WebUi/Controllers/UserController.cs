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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        /// <param name="repository">The repository.</param>
        public UserController(IBus bus, IRepository repository)
        {
            this.bus = bus;
            this.repository = repository;
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
        /// Logs user into the system.
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
        public ActionResult LogIn(string name, string password)
        {
            bus.Send(new LogIn
                {
                    UserId = repository
                        .GetAll<User>()
                        .Single(x => x.Name == name).Id,
                    Password = password
                });

            return RedirectToHomePage();
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
            bus.Send(new LogOut());

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
