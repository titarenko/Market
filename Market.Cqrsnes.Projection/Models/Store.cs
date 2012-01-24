using System;

namespace Market.Cqrsnes.Projection.Models
{
    public class Store
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public string OwnerName { get; set; }

        public int OffersCount { get; set; }
    }
}
