namespace Market.Cqrsnes.Projection
{
    /// <summary>
    /// Defines system context.
    /// </summary>
    public interface ISystemContext
    {
        /// <summary>
        /// Gets or sets principal.
        /// </summary>
        Principal Principal { get; set; }

        /// <summary>
        /// Gets or sets user.
        /// </summary>
        User User { get; set; }
    }

    /// <summary>
    /// Represents system context.
    /// </summary>
    public class SystemContext : ISystemContext
    {
        private User user;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemContext"/> class.
        /// </summary>
        public SystemContext()
        {
            Principal = new Principal(null);
        }

        /// <summary>
        /// Gets or sets principal.
        /// </summary>
        public Principal Principal { get; set; }

        /// <summary>
        /// Gets or sets user.
        /// </summary>
        public User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
                Principal = new Principal(user);
            }
        }
    }
}
