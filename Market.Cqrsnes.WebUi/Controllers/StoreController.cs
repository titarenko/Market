using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Controllers
{
    /// <summary>
    /// Hosts store-related actions.
    /// </summary>
    public class StoreController : Controller
    {
        private readonly IRepository repository;
        private readonly IBus bus;
        private readonly ISystemContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreController"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="bus">
        /// The bus.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public StoreController(IRepository repository, IBus bus, ISystemContext context)
        {
            this.repository = repository;
            this.bus = bus;
            this.context = context;
        }

        /// <summary>
        /// Lists stores.
        /// </summary>
        /// <returns>
        /// List of stores.
        /// </returns>
        public ActionResult List()
        {
            return View(repository.GetAll<Store>());
        }

        /// <summary>
        /// Lists offers for given store.
        /// </summary>
        /// <param name="id">
        /// The id of store.
        /// </param>
        /// <returns>
        /// List of offers.
        /// </returns>
        public ActionResult Offers(Guid id)
        {
            return View(repository.GetById<StoreOffers>(id));
        }

        /// <summary>
        /// Creates store.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Redirect to list of stores.
        /// </returns>
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
