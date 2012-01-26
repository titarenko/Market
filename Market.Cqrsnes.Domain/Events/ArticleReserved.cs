using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class ArticleReserved : Event
    {
        public Guid PurchaseId { get; set; }

        public int Count { get; set; }
    }
}