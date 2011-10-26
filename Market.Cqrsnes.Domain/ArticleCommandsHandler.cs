using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain
{
    public class ArticleCommandsHandler : 
        ICommandHandler<CreateArticle>,
        ICommandHandler<DeliverArticle>,
        ICommandHandler<BuyArticle>
    {
        private readonly IAggregateRootRepository repository;

        public ArticleCommandsHandler(IAggregateRootRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(CreateArticle command)
        {
            var instance = new Article(command.Id, command.Name);
            repository.Save(instance);
        }

        public void Handle(DeliverArticle command)
        {
            var instance = repository.GetById<Article>(command.Id);
            instance.Deliver(command.Count);
            repository.Save(instance);
        }

        public void Handle(BuyArticle command)
        {
            var instance = repository.GetById<Article>(command.Id);
            instance.Buy(command.Count);
            repository.Save(instance);
        }
    }
}