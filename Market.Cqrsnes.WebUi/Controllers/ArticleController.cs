using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Test;
using Market.Cqrsnes.Projection;
using Market.Cqrsnes.Projection.Test;
using Market.Cqrsnes.WebUi.Models;

namespace Market.Cqrsnes.WebUi.Controllers
{
    [HandleError]
    public class ArticleController : Controller
    {
        private readonly IBus bus;
        private readonly IRepository repository;

        public ArticleController(IBus bus, IRepository repository)
        {
            this.bus = bus;
            this.repository = repository;
        }

        public ActionResult List()
        {
            return View(repository.GetAll<Article>());
        }

        [HttpPost]
        public ActionResult ChangeCount(ChangeCountViewModel model)
        {
            if (ModelState.IsValid)
            {
                bus.Send(model.IsDelivery
                             ? (Command)new SupplyArticle
                             {
                                 OfferId = model.Id,
                                 Count = model.Count
                             }
                             : (Command)new BuyArticle
                             {
                                 OfferId = model.Id,
                                 Count = model.Count
                             });

            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult CreateArticle(CreateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bus.Send(new CreateArticle
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                });
            }

            return RedirectToAction("List");
        }

        public ActionResult DomainTest()
        {
            return View("Test", new /*ArticleSpecifications().ExecuteAll()*/object());
        }

        public ActionResult ProjectionTest()
        {
            return View("Test", new ArticleViewModelManagerSpecifications().ExecuteAll());
        }

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
