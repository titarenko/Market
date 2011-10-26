using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain
{
    public class ArticleDelivered : Event
    {
        public ArticleDelivered()
        {
        }

        public ArticleDelivered(Guid id, int count)
        {
            Id = id;
            Count = count;
        }

        public Guid Id { get; set; }

        public bool Equals(ArticleDelivered other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id) && other.Count == Count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ArticleDelivered)) return false;
            return Equals((ArticleDelivered) obj);
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
            return string.Format("Article delivered (Id: {0}, Count: {1})", Id, Count);
        }

        public int Count { get; set; }
    }
}