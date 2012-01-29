using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cqrsnes.Test;
using System.Linq;

namespace Market.Cqrsnes.WebUi.Controllers
{
    /// <summary>
    /// Represents set of miscellaneous actions without specific category.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IEnumerable<ISpecificationHolder> specifications;

        public HomeController(IEnumerable<ISpecificationHolder> specifications)
        {
            this.specifications = specifications;
        }

        /// <summary>
        /// Renders index page.
        /// </summary>
        /// <returns>
        /// Index page.
        /// </returns>
        public ActionResult Index()
        {
            return RedirectToAction("List", "Offer");
        }

        /// <summary>
        /// Renders error page.
        /// </summary>
        /// <returns>
        /// Error page.
        /// </returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Renders log page.
        /// </summary>
        /// <returns>
        /// Log page.
        /// </returns>
        public ActionResult Log()
        {
            try
            {
                var path = Server.MapPath("~/market.log");
                return View(System.IO.File.ReadAllText(path) as object);
            }
            catch (Exception)
            {
                return View();
            }
        }

        /// <summary>
        /// Lists all available system tests.
        /// </summary>
        /// <param name="id">
        /// Test identifier (assembly qualified name of its type).
        /// </param>
        /// <returns>
        /// List of system tests.
        /// </returns>
        public ActionResult Test()
        {
            return View(specifications);
        }

        public ActionResult TestResults(int id)
        {
            return View(specifications.Skip(id).First());
        }
    }
}
