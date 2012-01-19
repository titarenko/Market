using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Projection
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
        }
    }
}
