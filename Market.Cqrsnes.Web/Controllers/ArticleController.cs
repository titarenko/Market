using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain;
using Market.Cqrsnes.Test;
using Market.Cqrsnes.Web.Models;
using Market.Cqrsnes.Web.Service;

namespace Market.Cqrsnes.Web.Controllers
{
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
                             ? (Command) new DeliverArticle
                                             {
                                                 Id = model.Id,
                                                 Count = model.Count
                                             }
                             : (Command) new BuyArticle
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

        public ActionResult Test()
        {
            return View(new ArticleSpecifications().ExecuteAll());
        }
    }
}