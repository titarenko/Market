using System;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Domain.Entities
{
    public class Purchase : AggregateRoot,
        IChangeAcceptor<PurchaseCreated>
    {
        private int count;
        private Guid offerId;
        private Guid userId;
        private double amount;

        public Purchase()
        {
        }

        public Purchase(Guid id, Guid offerId, int count, double amount, Guid userId) : base(id)
        {
            ApplyChange(new PurchaseCreated
            {
                Id = id,
                OfferId = offerId,
                Count = count,
                Amount = amount,
                UserId = userId
            });
        }

        public void Accept(PurchaseCreated @event)
        {
            id = @event.Id;
            count = @event.Count;
            offerId = @event.OfferId;
            userId = @event.UserId;
            amount = @event.Amount;
        }

        public int GetCount()
        {
            return count;
        }

        public Guid GetOfferId()
        {
            return offerId;
        }

        public Guid GetUserId()
        {
            return userId;
        }

        public double GetAmount()
        {
            return amount;
        }
    }
}
