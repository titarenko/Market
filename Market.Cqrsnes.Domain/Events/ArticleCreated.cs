using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class ArticleCreated : Event
    {
        public ArticleCreated()
        {
        }

        public ArticleCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public bool Equals(ArticleCreated other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id) && Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ArticleCreated)) return false;
            return Equals((ArticleCreated) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode()*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Article created (Id: {0}, Name: {1})", Id, Name);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}