using System;
using System.Collections.Generic;

namespace Market.Cqrsnes.Projection.Models
{
    public class StoreOffers
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<Offer> Offers { get; set; }

        public Guid OwnerId { get; set; }

        public StoreOffers()
        {
            Offers = new List<Offer>();
        }
    }
}
