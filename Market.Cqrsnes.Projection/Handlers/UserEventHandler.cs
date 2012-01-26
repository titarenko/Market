using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection.Models;

namespace Market.Cqrsnes.Projection.Handlers
{
    /// <summary>
    /// Handles events related to <see cref="User"/>.
    /// </summary>
    public class UserEventHandler :
        IEventHandler<UserCreated>,
        IEventHandler<UserLoggedIn>,
        IEventHandler<UserLoggedOut>,
        IEventHandler<BalanceIncreased>,
        IEventHandler<BalanceDecreased>
    {
        private readonly ISystemContext context;
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEventHandler"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public UserEventHandler(ISystemContext context, IRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(UserCreated @event)
        {
            repository.Save(new User
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Balance = 0
                });
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(UserLoggedIn @event)
        {
            context.User = repository.GetById<User>(@event.UserId);
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
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

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(BalanceIncreased @event)
        {
            repository.Change<User>(
                @event.UserId, x => x.Balance += @event.Amount);
        }

        /// <summary>
        /// Handles (reacts to) event.
        /// </summary>
        /// <param name="event">Event instance.</param>
        public void Handle(BalanceDecreased @event)
        {
            repository.Change<User>(
                @event.UserId, x => x.Balance -= @event.Amount);
        }
    }
}
