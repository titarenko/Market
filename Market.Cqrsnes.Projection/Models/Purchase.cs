using System;

namespace Market.Cqrsnes.Projection.Models
{
    public class Purchase
    {
        public Guid Id { get; set; }

        public int Count { get; set; }

        public Guid StoreId { get; set; }
    }
}