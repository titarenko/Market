using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class BuyArticle : Command
    {
        public Guid BuyerId { get; set; }

        public Guid OfferId { get; set; }

        public int Count { get; set; }
    }
}
