using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Projection;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.WebUi.Controllers
{
    /// <summary>
    /// Hosts offer-related actions.
    /// </summary>
    public class OfferController : Controller
    {
        private readonly IBus bus;
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferController"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public OfferController(IBus bus, IRepository repository)
        {
            this.bus = bus;
            this.repository = repository;
        }

        /// <summary>
        /// Lists offers.
        /// </summary>
        /// <returns>
        /// List of offers.
        /// </returns>
        public ActionResult List()
        {
            return View(repository.GetAll<Offer>());
        }

        /// <summary>
        /// Creates offer.
        /// </summary>
        /// <param name="storeId">
        /// The store id.
        /// </param>
        /// <param name="articleId">
        /// The article id.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="price">
        /// The price.
        /// </param>
        /// <returns>
        /// Redirect to list of offers.
        /// </returns>
        [HttpPost]
        public ActionResult Create(Guid storeId, Guid articleId, int count, double price)
        {
            bus.Send(new CreateOffer
                {
                    Id = Guid.NewGuid(),
                    StoreId = storeId,
                    ArticleId = articleId,
                    Count = count,
                    Price = price
                });

            return RedirectToAction("List");
        }
    }
}
