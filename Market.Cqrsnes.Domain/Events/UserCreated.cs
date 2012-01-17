using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    /// <summary>
    /// Indicates that user was created.
    /// </summary>
    public class UserCreated : Event
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
