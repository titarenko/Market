using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class CreateOffer : Command
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public Guid ArticleId { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }
    }
}
