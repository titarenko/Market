﻿using System;

namespace Market.Cqrsnes.Projection
{
    public class Store
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public string OwnerName { get; set; }
    }
}