using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Controllers
{
    /// <summary>
    /// Hosts article-related actions.
    /// </summary>
    public class ArticleController : Controller
    {
        private readonly IBus bus;
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public ArticleController(IBus bus, IRepository repository)
        {
            this.bus = bus;
            this.repository = repository;
        }

        /// <summary>
        /// Lists articles.
        /// </summary>
        /// <returns>
        /// List of articles.
        /// </returns>
        public ActionResult List()
        {
            return View(repository.GetAll<Article>());
        }

        /// <summary>
        /// Creates new article.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Redirect to list of articles.
        /// </returns>
        [HttpPost]
        public ActionResult Create(string name)
        {
            bus.Send(new CreateArticle
                {
                    Id = Guid.NewGuid(),
                    Name = name
                });

            return RedirectToAction("List");
        }
    }
}
