using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Entities;
using Market.Cqrsnes.Domain.Utility;

namespace Market.Cqrsnes.Domain.Handlers
{
    /// <summary>
    /// Handles commands related to <see cref="User"/>.
    /// </summary>
    public class UserCommandHandler : 
        ICommandHandler<CreateUser>, 
        ICommandHandler<LogIn>,
        ICommandHandler<LogOut>
    {
        private readonly IAggregateRootRepository repository;
        private readonly IPasswordHashGenerator generator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="generator">The generator.</param>
        public UserCommandHandler(IAggregateRootRepository repository, IPasswordHashGenerator generator)
        {
            this.repository = repository;
            this.generator = generator;
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(CreateUser command)
        {
            var user = new User(command.Id, command.Name);
            user.SetPassword(command.Password, generator);
            repository.Save(user);
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(LogIn command)
        {
            repository.PerformAction<User>(
                command.UserId, x => x.LogIn(command.Password, generator));
        }

        /// <summary>
        /// Handles (reacts to) command.
        /// </summary>
        /// <param name="command">Command instance.</param>
        public void Handle(LogOut command)
        {
            repository.PerformAction<User>(
                command.UserId, x => x.LogOut());
        }
    }
}
