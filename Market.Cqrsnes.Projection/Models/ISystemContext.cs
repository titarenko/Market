using System.Security.Principal;

namespace Market.Cqrsnes.Projection.Models
{
    /// <summary>
    /// Defines system context.
    /// </summary>
    public interface ISystemContext
    {
        /// <summary>
        /// Gets principal.
        /// </summary>
        IPrincipal Principal { get; }

        /// <summary>
        /// Gets or sets user.
        /// </summary>
        User User { get; set; }
    }
}
