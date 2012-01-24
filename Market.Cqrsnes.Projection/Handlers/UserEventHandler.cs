using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.Projection.Handlers
{
    public class UserEventHandler :
        IEventHandler<UserCreated>,
        IEventHandler<UserLoggedIn>,
        IEventHandler<UserLoggedOut>,
        IEventHandler<BalanceIncreased>
    {
        private readonly ISystemContext context;
        private readonly IRepository repository;

        public UserEventHandler(ISystemContext context, IRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public void Handle(UserCreated @event)
        {
            repository.Save(new User
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Balance = 0
                });
        }

        public void Handle(UserLoggedIn @event)
        {
            context.User = repository.GetById<User>(@event.UserId);
        }

        public void Handle(UserLoggedOut @event)
        {
            if (context.Principal.Identity.IsAuthenticated && context.User.Id == @event.UserId)
            {
                context.User = null;
            }
            else
            {
                throw new PossibleConcurrencyProblemException();
            }
        }

        public void Handle(BalanceIncreased @event)
        {
            repository.Change<User>(
                @event.UserId, x => x.Balance += @event.Amount);
        }
    }
}
