using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    public class CreateArticle : Command
    {
        public CreateArticle()
        {
        }

        public CreateArticle(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("Creating article (Id: {0}, Name: {1})", Id, Name);
        }
    }
}