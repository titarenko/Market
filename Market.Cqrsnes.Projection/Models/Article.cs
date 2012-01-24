using System;

namespace Market.Cqrsnes.Projection.Models
{
    public class Article
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int OffersCount { get; set; }
    }
}
