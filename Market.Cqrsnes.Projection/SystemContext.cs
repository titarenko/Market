namespace Market.Cqrsnes.Projection
{
    public interface ISystemContext
    {
        Principal Principal { get; set; }
        User User { get; set; }
    }

    public class SystemContext : ISystemContext
    {
        private User user;

        public SystemContext()
        {
            Principal = new Principal(null);
        }

        public Principal Principal { get; set; }
        
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
