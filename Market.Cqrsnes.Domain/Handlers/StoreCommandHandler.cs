using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Entities;

namespace Market.Cqrsnes.Domain.Handlers
{
    /// <summary>
    /// Handles commands related to <see cref="Store"/>.
    /// </summary>
    public class StoreCommandHandler :
        ICommandHandler<CreateStore>
    {
        private readonly IAggregateRootRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public StoreCommandHandler(IAggregateRootRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(CreateStore command)
        {
            repository.Save(new Store(command.Id, command.Name));
        }
    }
}
