using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class OfferPriceChanged : Event
    {
        public Guid Id { get; set; }

        public double Price { get; set; }
    }
}
