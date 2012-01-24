using System;

namespace Market.Cqrsnes.Projection.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }
    }
}
