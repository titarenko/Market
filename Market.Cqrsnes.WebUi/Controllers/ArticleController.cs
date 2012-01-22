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
using IDependencyResolver = Cqrsnes.Infrastructure.IDependencyResolver;

namespace Market.Cqrsnes.WebUi.Controllers
{
    [HandleError]
    public class ArticleController : Controller
    {
        private readonly IBus bus;
        private readonly IDependencyResolver resolver;

        public ArticleController(IBus bus, IDependencyResolver resolver)
        {
            this.bus = bus;
            this.resolver = resolver;
        }

        public ActionResult List()
        {
            var manager = resolver.Resolve<ArticleViewModelManager>();
            return View(manager.GetArticleListViewModel());
        }

        [HttpPost]
        public ActionResult ChangeCount(ChangeCountViewModel model)
        {
            if (ModelState.IsValid)
            {
                bus.Send(model.IsDelivery
                             ? (Command)new SupplyArticle
                             {
                                 Id = model.Id,
                                 Count = model.Count
                             }
                             : (Command)new BuyArticle
                             {
                                 Id = model.Id,
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
            return View("Test", new ArticleSpecifications().ExecuteAll());
        }

        public ActionResult ProjectionTest()
        {
            return View("Test", new ArticleViewModelManagerSpecifications().ExecuteAll());
        }
    }
}