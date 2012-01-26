using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.Projection.Handlers
{
    public class ArticleEventHandler :
        IEventHandler<ArticleCreated>
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
        public void Handle(ArticleCreated @event)
        {
            repository.Save(new Article
                {
                    Id = @event.Id,
                    Name = @event.Name
                });
        }
    }
}
