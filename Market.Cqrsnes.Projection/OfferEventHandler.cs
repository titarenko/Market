using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Projection
{
    /// <summary>
    /// Handles offer-related events.
    /// </summary>
    public class OfferEventHandler :
        IEventHandler<OfferCreated>
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
            repository.Change<StoreOffers>(@event.StoreId, x => x.Offers.Add(offer));
        }
    }
}
