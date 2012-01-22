using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    /// <summary>
    /// Indicates that reservation of article is canceled.
    /// </summary>
    public class ReservationCanceled : Event
    {
        public Guid OfferId { get; set; }

        public Guid CustomerId { get; set; }

        public int Count { get; set; }
    }
}
