using System;

namespace Market.Cqrsnes.Projection
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}