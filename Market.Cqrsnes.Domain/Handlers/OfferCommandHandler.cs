using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Entities;

namespace Market.Cqrsnes.Domain.Handlers
{
    /// <summary>
    /// Handles offer-related commands.
    /// </summary>
    public class OfferCommandHandler :
        ICommandHandler<CreateOffer>,
        ICommandHandler<ReserveArticle>
    {
        private readonly IAggregateRootRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public OfferCommandHandler(IAggregateRootRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(CreateOffer command)
        {
            repository.Save(new Offer(
                                command.Id,
                                command.StoreId,
                                command.ArticleId,
                                command.Price,
                                command.Count));
        }

        public void Handle(ReserveArticle command)
        {
            var purchase = repository.GetById<Purchase>(command.PurchaseId);

            repository.PerformAction<Offer>(
                purchase.GetOfferId(),
                x => x.Reserve(purchase.GetCount(), command.PurchaseId));

            repository.Save(purchase);
        }
    }
}