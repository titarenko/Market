using System;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Entities;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Domain.Handlers
{
    /// <summary>
    /// Represents purchase process.
    /// </summary>
    /// <remarks>
    /// Purchase process consists of following steps:
    /// 1) reservation of buyer's money;
    /// 2) reservation of article of 1) succeeded;
    /// 3.1) decrease of buyer's balance if 2) succeeded;
    /// 3.2) cancelation of decrease of buyer's balance if 2) failed;
    /// 4.1) decrease of article units count if 3.1) succeeded;
    /// 4.2) cancelation of reservation of article if 3.1) failed.
    /// </remarks>
    public class PurchaseSaga :
        ICommandHandler<BuyArticle>,
        IEventHandler<MoneyReserved>,
        IEventHandler<ArticleReserved>,
        IEventHandler<BalanceDecreased>,
        IEventHandler<BalanceDecreaseFailed>
    {
        private readonly IAggregateRootRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseSaga"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public PurchaseSaga(IAggregateRootRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(BuyArticle command)
        {
            var buyer = repository.GetById<User>(command.BuyerId);
            var offer = repository.GetById<Offer>(command.OfferId);
            
            var amount = offer.CalculatePrice(command.Count);

            var purchaseId = Guid.NewGuid();
            repository.Save(new Purchase(
                                purchaseId,
                                command.OfferId,
                                command.Count,
                                amount,
                                command.BuyerId));

            buyer.ReserveMoney(amount, purchaseId);

            repository.Save(buyer);
            repository.Save(offer);
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(MoneyReserved @event)
        {
            var purchase = repository.GetById<Purchase>(@event.PurchaseId);

            repository.PerformAction<Offer>(
                purchase.GetOfferId(),
                x => x.Reserve(purchase.GetCount(), @event.PurchaseId));

            repository.Save(purchase);
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(ArticleReserved @event)
        {
            var purchase = repository.GetById<Purchase>(@event.PurchaseId);

            repository.PerformAction<User>(
                purchase.GetUserId(),
                x => x.DecreaseBalance(purchase.GetAmount(), @event.PurchaseId));

            repository.Save(purchase);
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(BalanceDecreased @event)
        {
            var purchase = repository.GetById<Purchase>(@event.PurchaseId);

            repository.PerformAction<Offer>(
                purchase.GetOfferId(), 
                x => x.DecreaseCount(purchase.GetCount(), @event.PurchaseId));

            repository.Save(purchase);
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(BalanceDecreaseFailed @event)
        {
            var purchase = repository.GetById<Purchase>(@event.PurchaseId);

            repository.PerformAction<Offer>(
                purchase.GetOfferId(),
                x => x.CancelReservation(purchase.GetCount(), @event.PurchaseId));

            repository.Save(purchase);
        }
    }
}
