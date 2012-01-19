using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Controllers
{
    public class StoreController : Controller
    {
        private readonly IRepository repository;

        public StoreController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult List()
        {
            return View(repository.GetAll<Store>());
        }

        public ActionResult Offers(Guid id)
        {
            return View(repository.GetSingle<StoreOffers>());
        }
    }
}
