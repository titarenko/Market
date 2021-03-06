﻿using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Commands
{
    /// <summary>
    /// Specifies to do log out.
    /// </summary>
    public class LogOut : Command
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
