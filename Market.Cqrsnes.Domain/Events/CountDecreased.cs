using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class CountDecreased : Event
    {
        public Guid OfferId { get; set; }

        public Guid PurchaseId { get; set; }
    }
}