using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class PurchaseCreated : Event
    {
        public Guid Id { get; set; }

        public Guid OfferId { get; set; }

        public int Count { get; set; }

        public Guid UserId { get; set; }

        public double Amount { get; set; }
    }
}