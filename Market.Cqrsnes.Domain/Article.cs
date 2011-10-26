using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain
{
    public class Article : 
        AggregateRoot,
        IChangeAcceptor<ArticleCreated>,
        IChangeAcceptor<ArticleDelivered>,
        IChangeAcceptor<ArticleBought>
    {
        private int count;

        public Article()
        {
        }

        public Article(Guid id, string name)
            : base(id)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Can't assign null name.");
            }

            ApplyChange(new ArticleCreated
                            {
                                Id = id,
                                Name = name
                            });
        }

        public void Deliver(int count)
        {
            if (count <= 0)
            {
                throw new InvalidOperationException("Can't deliver non-positive count of units.");
            }

            ApplyChange(new ArticleDelivered
                            {
                                Id = Id,
                                Count = count
                            });
        }

        public void Buy(int count)
        {
            if (count > this.count)
            {
                throw new InvalidOperationException("Can't buy more units than exist.");
            }

            ApplyChange(new ArticleBought
                            {
                                Id = Id,
                                Count = count
                            });
        }

        public void Accept(ArticleCreated @event)
        {
            id = @event.Id;
        }

        public void Accept(ArticleDelivered @event)
        {
            count += @event.Count;
        }

        public void Accept(ArticleBought @event)
        {
            count -= @event.Count;
        }
    }
}