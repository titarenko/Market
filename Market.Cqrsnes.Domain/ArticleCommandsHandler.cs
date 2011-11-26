using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain
{
    /// <summary>
    /// Handles commands related to <see cref="Article"/>.
    /// </summary>
    public class ArticleCommandsHandler : 
        ICommandHandler<CreateArticle>,
        ICommandHandler<DeliverArticle>,
        ICommandHandler<BuyArticle>
    {
        private readonly IAggregateRootRepository repository;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="repository">Aggregate root repository.</param>
        public ArticleCommandsHandler(IAggregateRootRepository repository)
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
            instance.Deliver(command.Count);
            repository.Save(instance);
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(BuyArticle command)
        {
            var instance = repository.GetById<Article>(command.Id);
            instance.Buy(command.Count);
            repository.Save(instance);
        }
    }
}