using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    /// <summary>
    /// Indicates that store was created.
    /// </summary>
    public class StoreCreated : Event
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }
    }
}
