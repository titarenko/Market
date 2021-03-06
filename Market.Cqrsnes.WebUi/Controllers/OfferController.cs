﻿using System;
using System.Web.Mvc;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
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
        private readonly ISystemContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferController"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="context">The context.</param>
        public OfferController(IBus bus, IRepository repository, ISystemContext context)
        {
            this.bus = bus;
            this.repository = repository;
            this.context = context;
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
            if (context.User.Id != repository.GetById<Store>(storeId).OwnerId)
            {
                throw new ApplicationException("Only owner can create the offer.");
            }

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

        /// <summary>
        /// Initiates purchase and redirects to list of offers.
        /// </summary>
        /// <param name="offerId">
        /// The offer id.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// Redirect to list of offers.
        /// </returns>
        [HttpPost]
        public ActionResult Buy(Guid offerId, int count)
        {
            bus.Send(new BuyArticle
                {
                    OfferId = offerId,
                    Count = count,
                    BuyerId = context.User.Id
                });

            return RedirectToAction("List");
        }
    }
}
