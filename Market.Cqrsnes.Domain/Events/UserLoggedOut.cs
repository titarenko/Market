using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    public class UserLoggedOut : Event
    {
        public Guid UserId { get; set; }
    }
}