using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Entities;

namespace Market.Cqrsnes.Domain.Handlers
{
    /// <summary>
    /// Handles commands related to <see cref="Article"/>.
    /// </summary>
    public class ArticleCommandHandler : 
        ICommandHandler<CreateArticle>,
        ICommandHandler<DeliverArticle>,
        ICommandHandler<BuyArticle>
    {
        private readonly IAggregateRootRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public ArticleCommandHandler(IAggregateRootRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(CreateArticle command)
        {
            var instance = new Article(command.Id, command.Name);
            repository.Save(instance);
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(DeliverArticle command)
        {
            var instance = repository.GetById<Article>(command.Id);
            //instance.Deliver(command.Count);
            repository.Save(instance);
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(BuyArticle command)
        {
            var instance = repository.GetById<Article>(command.Id);
            //instance.Buy(command.Count);
            repository.Save(instance);
        }
    }
}