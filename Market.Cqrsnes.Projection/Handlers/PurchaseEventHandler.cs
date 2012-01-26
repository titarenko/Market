using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.Projection.Handlers
{
    /// <summary>
    /// Handles events related to <see cref="Purchase"/>.
    /// </summary>
    public class PurchaseEventHandler :
        IEventHandler<PurchaseCreated>
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseEventHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public PurchaseEventHandler(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(PurchaseCreated @event)
        {
            repository.Save(new Purchase
                {
                    Id = @event.Id,
                    StoreId = repository.GetById<Offer>(@event.OfferId).StoreId,
                    Count = @event.Count
                });
        }
    }
}
