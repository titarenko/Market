using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    /// <summary>
    /// Specifies that user should be logged in.
    /// </summary>
    public class LogIn : Command
    {
        public Guid UserId { get; set; }

        public string Password { get; set; }
    }
}
