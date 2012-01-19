﻿using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Projection
{
    public class ArticleEventHandler :
        IEventHandler<OfferCreated>,
        IEventHandler<OfferPriceChanged>
    {
        private readonly IRepository repository;

        public ArticleEventHandler(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(OfferCreated @event)
        {
            repository.Save(new Article
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    StoreId = @event.StoreId,
                    StoreName = repository.GetById<Store>(@event.StoreId).Name,
                    Count = 0,
                    Price = 0
                });
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(OfferPriceChanged @event)
        {
            repository.Change<Offer>(
                @event.Id,
                offer => offer.Price = @event.Price);
        }
    }
}