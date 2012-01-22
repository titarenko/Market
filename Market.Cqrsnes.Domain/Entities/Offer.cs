using System;
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
        IChangeAcceptor<ArticleSupplied>,
        IChangeAcceptor<ArticleReserved>,
        IChangeAcceptor<PriceChanged>,
        IChangeAcceptor<ReservationCanceled>
    {
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
        /// Increases count of article units.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        public void Supply(int count)
        {
            count.ShouldBePositive("count");

            ApplyChange(new ArticleSupplied
                {
                    OfferId = id,
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
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        public void CancelReservation(int count, Guid customerId)
        {
            count.ShouldBePositive("count");

            ApplyChange(new ReservationCanceled
                {
                    OfferId = id,
                    CustomerId = customerId,
                    Count = count
                });
        }

        /// <summary>
        /// Changes price.
        /// </summary>
        /// <param name="price">
        /// The price.
        /// </param>
        public void ChangePrice(double price)
        {
            price.ShouldBePositive("price");

            ApplyChange(new PriceChanged
                {
                    OfferId = id,
                    Price = price
                });
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
        public void Accept(ArticleSupplied @event)
        {
            count += @event.Count;
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(ArticleReserved @event)
        {
            count -= @event.Count;
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(PriceChanged @event)
        {
            price = @event.Price;
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(ReservationCanceled @event)
        {
            count += @event.Count;
        }
    }
}
