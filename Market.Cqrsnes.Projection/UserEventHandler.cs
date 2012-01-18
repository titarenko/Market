using System;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Entities;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Projection
{
    public class UserEventHandler :
        IEventHandler<UserCreated>,
        IEventHandler<UserLoggedIn>,
        IEventHandler<UserLoggedOut>
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
    }
}
