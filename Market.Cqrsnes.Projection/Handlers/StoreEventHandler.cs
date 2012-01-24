using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.Projection.Handlers
{
    public class StoreEventHandler :
        IEventHandler<StoreCreated>
    {
        private readonly IRepository repository;

        public StoreEventHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(StoreCreated @event)
        {
            repository.Save(new Store
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    OwnerId = @event.OwnerId,
                    OwnerName = repository.GetById<User>(@event.OwnerId).Name
                });

            repository.Save(new StoreOffers
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Offers = new List<Offer>(),
                    OwnerId = @event.OwnerId
                });
        }
    }
}
