using System;
using System.Collections.Generic;

namespace Market.Cqrsnes.Projection
{
    public class UserList
    {
        public UserList()
        {
            IdByName = new Dictionary<string, Guid>();
        }

        public IDictionary<string, Guid> IdByName { get; private set; }
    }
}
