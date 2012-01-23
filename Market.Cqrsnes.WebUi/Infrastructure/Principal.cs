using System.Security.Principal;
using Market.Cqrsnes.Projection;

namespace Market.Cqrsnes.WebUi.Infrastructure
{
    public class Principal : IPrincipal
    {
        private Identity identity;

        public Principal(User user)
        {
            identity = new Identity(user);
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }
    }
}
