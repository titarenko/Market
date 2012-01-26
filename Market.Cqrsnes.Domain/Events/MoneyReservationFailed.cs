using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class MoneyReservationFailed : Event
    {
        public Guid UserId { get; set; }

        public double Amount { get; set; }

        public Guid PurchaseId { get; set; }

        public int Count { get; set; }
    }
}