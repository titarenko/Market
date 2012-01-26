using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class ArticleReservationFailed : Event
    {
        public Guid PurchaseId { get; set; }
    }
}