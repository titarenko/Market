using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.Projection.Handlers
{
    /// <summary>
    /// Handles events related to <see cref="Offer"/>.
    /// </summary>
    public class OfferEventHandler :
        IEventHandler<OfferCreated>,
        IEventHandler<CountDecreased>
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferEventHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public OfferEventHandler(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(OfferCreated @event)
        {
            var offer = new Offer
                {
                    Id = @event.Id,
                    StoreId = @event.StoreId,
                    StoreName = repository.GetById<Store>(@event.StoreId).Name,
                    ArticleId = @event.ArticleId,
                    ArticleName = repository.GetById<Article>(@event.ArticleId).Name,
                    Price = @event.Price,
                    Count = @event.Count
                };

            repository.Save(offer);

            repository.Change<Store>(@event.StoreId, x => x.OffersCount++);
            repository.Change<StoreOffers>(@event.StoreId, x => x.Offers.Add(offer));
            repository.Change<Article>(@event.ArticleId, x => x.OffersCount++);
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(CountDecreased @event)
        {
            var purchase = repository.GetById<Purchase>(@event.PurchaseId);
            repository.Change<Offer>(@event.OfferId, x => x.Count -= purchase.Count);
        }
    }
}
