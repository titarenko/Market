using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class ArticleReserved : Event
    {
        public Guid OfferId { get; set; }

        public Guid CustomerId { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }
    }
}