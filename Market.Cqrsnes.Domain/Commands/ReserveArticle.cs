using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class ReserveArticle : Command
    {
        public Guid PurchaseId { get; set; }
    }
}
