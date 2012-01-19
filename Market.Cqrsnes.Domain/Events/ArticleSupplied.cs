using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class ArticleSupplied : Event
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public int Count { get; set; }
    }
}
