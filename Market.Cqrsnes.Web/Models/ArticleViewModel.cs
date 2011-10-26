using System;

namespace Market.Cqrsnes.Web.Models
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}