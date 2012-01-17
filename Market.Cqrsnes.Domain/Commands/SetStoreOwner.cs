using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class SetStoreOwner : Command
    {
        public Guid StoreId { get; set; }

        public Guid OwnerId { get; set; }
    }
}