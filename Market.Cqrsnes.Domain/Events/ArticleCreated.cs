using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class ArticleCreated : Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}