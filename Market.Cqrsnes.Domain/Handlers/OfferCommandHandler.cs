using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Entities;

namespace Market.Cqrsnes.Domain.Handlers
{
    /// <summary>
    /// Handles offer-related commands.
    /// </summary>
    public class OfferCommandHandler :
        ICommandHandler<CreateOffer>
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
    }
}