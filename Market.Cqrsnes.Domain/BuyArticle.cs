using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain
{
    public class BuyArticle : Command
    {
        public BuyArticle()
        {
        }

        public BuyArticle(Guid id, int count)
        {
            Id = id;
            Count = count;
        }

        public Guid Id { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return string.Format("Buying article (Id: {0}, Count: {1})", Id, Count);
        }
    }
}