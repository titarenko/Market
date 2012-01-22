using System;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Utility;

namespace Market.Cqrsnes.Domain.Entities
{
    /// <summary>
    /// Represents article (product, goods, whatever you want).
    /// </summary>
    public class Article : AggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Article"/> class.
        /// </summary>
        public Article()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Article"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public Article(Guid id, string name) : base(id)
        {
            name.ShouldNotBeEmpty("name");

            ApplyChange(new ArticleCreated
                            {
                                Id = id,
                                Name = name
                            });
        }
    }
}
