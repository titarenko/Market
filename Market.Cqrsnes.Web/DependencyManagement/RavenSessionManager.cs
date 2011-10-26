using Raven.Client;

namespace Market.Cqrsnes.Web.DependencyManagement
{
    public class RavenSessionManager
    {
        private readonly IDocumentStore store;
        private IDocumentSession session;

        public RavenSessionManager(IDocumentStore store)
        {
            this.store = store;
        }

        public void StartSession()
        {
            if (session != null) return;

            session = store.OpenSession();
        }

        public void StopSession()
        {
            if (session == null) return;

            session.Dispose();
            session = null;
        }

        public IDocumentSession Session
        {
            get
            {
                return session ?? (session = store.OpenSession());
            }
        }
    }
}