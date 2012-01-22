using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class PriceChanged : Event
    {
        public Guid OfferId { get; set; }

        public double Price { get; set; }
    }
}
