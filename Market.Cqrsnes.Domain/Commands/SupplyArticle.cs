using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class SupplyArticle : Command
    {
        public Guid OfferId { get; set; }

        public int Count { get; set; }
    }
}
