using System;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Domain.Entities
{
    /// <summary>
    /// Represents store - place where articles can be bought.
    /// </summary>
    public class Store : AggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Store"/> class.
        /// </summary>
        public Store()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Store"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public Store(Guid id, string name) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Name should not be empty.", "name");
            }

            ApplyChange(new StoreCreated
                {
                    Id = id,
                    Name = name
                });
        }
    }
}
