using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    /// <summary>
    /// Specifies to do log in.
    /// </summary>
    public class LogIn : Command
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }
    }
}
