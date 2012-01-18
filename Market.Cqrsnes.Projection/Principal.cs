using System.Security.Principal;

namespace Market.Cqrsnes.Projection
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
