using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Controllers
{
    public class StoreController : Controller
    {
        private readonly IRepository repository;
        private readonly IBus bus;

        public StoreController(IRepository repository, IBus bus)
        {
            this.repository = repository;
            this.bus = bus;
        }

        public ActionResult List()
        {
            return View(repository.GetAll<Store>());
        }

        public ActionResult Offers(Guid id)
        {
            return View(repository.GetSingle<StoreOffers>());
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            bus.Send(new CreateStore
                {
                    Id = Guid.NewGuid(),
                    Name = name
                });

            return RedirectToAction("List");
        }
    }
}
