using System.Security.Principal;
using Market.Cqrsnes.Projection;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.WebUi.Infrastructure
{
    public class Identity : IIdentity
    {
        private readonly User user;

        public Identity(User user)
        {
            this.user = user;
        }

        public string Name
        {
            get { return user.Name; }
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return user != null; }
        }
    }
}
