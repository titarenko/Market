using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class DeliverArticle : Command
    {
        public DeliverArticle()
        {
        }

        public DeliverArticle(Guid id, int count)
        {
            Id = id;
            Count = count;
        }

        public Guid Id { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return string.Format("Delivering article (Id: {0}, Count: {1})", Id, Count);
        }
    }
}