using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class BalanceDecreaseFailed : Event
    {
        public Guid UserId { get; set; }

        public Guid PurchaseId { get; set; }

        public double Amount { get; set; }
    }
}