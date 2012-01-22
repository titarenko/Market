using System;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Utility;

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
        /// <param name="ownerId">The owner id.</param>
        public Store(Guid id, string name, Guid ownerId) : base(id)
        {
            name.ShouldNotBeEmpty("name");
            ownerId.ShouldNotBeEmpty("ownerId");

            ApplyChange(new StoreCreated
                {
                    Id = id,
                    Name = name,
                    OwnerId = ownerId
                });
        }
    }
}
