using System;

namespace Market.Cqrsnes.Projection
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }
    }
}
