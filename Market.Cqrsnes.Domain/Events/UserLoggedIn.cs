using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class UserLoggedIn : Event
    {
        public Guid UserId { get; set; }
    }
}