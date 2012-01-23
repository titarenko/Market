using System;
using System.Collections.Generic;

namespace Market.Cqrsnes.Projection
{
    public class StoreOffers
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<Offer> Offers { get; set; }
    }
}
