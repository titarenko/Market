using System;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Utility;

namespace Market.Cqrsnes.Domain.Entities
{
    /// <summary>
    /// Represents offer.
    /// </summary>
    public class Offer : AggregateRoot,
        IChangeAcceptor<OfferCreated>,
        IChangeAcceptor<ArticleReserved>,
        IChangeAcceptor<ReservationCanceled>,
        IChangeAcceptor<CountDecreased>
    {
        private readonly IList<Guid> pendingPurchases = new List<Guid>();
        private double price;
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Offer"/> class.
        /// </summary>
        public Offer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Offer"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="storeId">
        /// The store id.
        /// </param>
        /// <param name="articleId">
        /// The article id.
        /// </param>
        /// <param name="price">
        /// The price.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        public Offer(Guid id, Guid storeId, Guid articleId, double price, int count) : base(id)
        {
            storeId.ShouldNotBeEmpty("storeId");
            articleId.ShouldNotBeEmpty("articleId");
            price.ShouldBePositive("price");
            count.ShouldBePositive("count");

            ApplyChange(new OfferCreated
                {
                    Id = id,
                    StoreId = storeId,
                    ArticleId = articleId,
                    Price = price,
                    Count = count
                });
        }

        /// <summary>
        /// Reserves certain amount of article for given customer.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        public void Reserve(int count, Guid customerId)
        {
            count.ShouldBePositive("count");

            if (count > this.count)
            {
                ApplyChange(new ArticleReservationFailed
                    {
                        OfferId = id,
                        CustomerId = customerId,
                        Count = count
                    });
            }
            else
            {
                ApplyChange(new ArticleReserved
                    {
                        OfferId = id,
                        CustomerId = customerId,
                        Count = count,
                        Price = price
                    });
            }
        }

        /// <summary>
        /// Cancels reservation for given customer.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="purchaseId">
        /// The customer id.
        /// </param>
        public void CancelReservation(int count, Guid purchaseId)
        {
            count.ShouldBePositive("count");
            purchaseId.ShouldNotBeEmpty("purchaseId");

            if (pendingPurchases.Contains(purchaseId))
            {
                ApplyChange(new ReservationCanceled
                {
                    OfferId = id,
                    PurchaseId = purchaseId,
                    Count = count
                });
            }

            throw new ApplicationException(
                "Can't cancel reservation for unknown purchase.");
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(OfferCreated @event)
        {
            id = @event.Id;
            count = @event.Count;
            price = @event.Price;
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(ArticleReserved @event)
        {
            count -= @event.Count;
            pendingPurchases.Add(@event.PurchaseId);
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(ReservationCanceled @event)
        {
            count += @event.Count;
            pendingPurchases.Remove(@event.PurchaseId);
        }

        public double CalculatePrice(int count)
        {
            return price * count;
        }

        public void DecreaseCount(int count, Guid purchaseId)
        {
            if (pendingPurchases.Contains(purchaseId))
            {
                ApplyChange(new CountDecreased
                    {
                        OfferId = id,
                        PurchaseId = purchaseId
                    });
            }
        }

        public void Accept(CountDecreased @event)
        {
            pendingPurchases.Remove(@event.PurchaseId);
        }
    }

    public class CountDecreased : Event
    {
        public Guid OfferId { get; set; }

        public Guid PurchaseId { get; set; }
    }
}
