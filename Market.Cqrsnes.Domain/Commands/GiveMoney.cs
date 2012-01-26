using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    /// <summary>
    /// Specifies that user's balance should be increased.
    /// </summary>
    public class GiveMoney : Command
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
