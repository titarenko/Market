using System;

namespace Market.Cqrsnes.Projection.Models
{
    public class Offer
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }

        public string ArticleName { get; set; }
        
        public Guid StoreId { get; set; }

        public string StoreName { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }
    }
}
