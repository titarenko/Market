using System;

namespace Market.Cqrsnes.Projection
{
    public class Article
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid StoreId { get; set; }

        public string StoreName { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }
    }
}
