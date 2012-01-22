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
        private readonly ISystemContext context;

        public StoreController(IRepository repository, IBus bus, ISystemContext context)
        {
            this.repository = repository;
            this.bus = bus;
            this.context = context;
        }

        public ActionResult List()
        {
            return View(repository.GetAll<Store>());
        }

        public ActionResult Offers(Guid id)
        {
            return View(repository.GetById<StoreOffers>(id));
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            bus.Send(new CreateStore
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    OwnerId = context.User.Id
                });

            return RedirectToAction("List");
        }
    }
}
