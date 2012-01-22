using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class CreateArticle : Command
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
