using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Events
{
    /// <summary>
    /// Indicates that user password was set.
    /// </summary>
    [VisibleWithinDeclaringAssemblyOnly]
    public class UserPasswordSet : Event
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets password hash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets password salt.
        /// </summary>
        public string PasswordSalt { get; set; }
    }
}
