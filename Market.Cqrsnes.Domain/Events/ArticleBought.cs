using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class ArticleBought : Event
    {
        public ArticleBought()
        {
        }

        public ArticleBought(Guid id, int count)
        {
            Id = id;
            Count = count;
        }

        public bool Equals(ArticleBought other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id) && other.Count == Count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ArticleBought)) return false;
            return Equals((ArticleBought) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode()*397) ^ Count;
            }
        }

        public override string ToString()
        {
            return string.Format("Article bought (Id: {0}, Count: {1})", Id, Count);
        }

        public Guid Id { get; set; }
        public int Count { get; set; }
    }
}