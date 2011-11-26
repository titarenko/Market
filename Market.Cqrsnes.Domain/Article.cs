using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain
{
    /// <summary>
    /// Represents article (product, goods, whatever you want).
    /// </summary>
    public class Article : 
        AggregateRoot,
        IChangeAcceptor<ArticleCreated>,
        IChangeAcceptor<ArticleDelivered>,
        IChangeAcceptor<ArticleBought>
    {
        private int count;

        /// <summary>
        /// Constructs new instance.
        /// </summary>
        public Article()
        {
        }

        /// <summary>
        /// Constructs new instance.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
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

        /// <summary>
        /// Delivers (adds to some virtual storage) certain qty of article.
        /// </summary>
        /// <param name="count">Number of units.</param>
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

        /// <summary>
        /// Buys (removes from some virtual storage) certain qty of article.
        /// </summary>
        /// <param name="count">Number of units.</param>
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

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event"></param>
        public void Accept(ArticleCreated @event)
        {
            id = @event.Id;
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event"></param>
        public void Accept(ArticleDelivered @event)
        {
            count += @event.Count;
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event"></param>
        public void Accept(ArticleBought @event)
        {
            count -= @event.Count;
        }
    }
}