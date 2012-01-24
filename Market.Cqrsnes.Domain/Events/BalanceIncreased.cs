using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    /// <summary>
    /// Indicates that balance of certain user was increased.
    /// </summary>
    public class BalanceIncreased : Event
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets amount.
        /// </summary>
        public double Amount { get; set; }
    }
}
