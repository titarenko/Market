using System;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Utility;

namespace Market.Cqrsnes.Domain.Entities
{
    /// <summary>
    /// Represents user of the system.
    /// </summary>
    public class User : AggregateRoot,
        IChangeAcceptor<UserPasswordSet>
    {
        private string passwordHash;
        private string passwordSalt;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public User(Guid id, string name) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name can not be empty.", "name");
            }

            ApplyChange(new UserCreated
                {
                    Id = id,
                    Name = name
                });
        }

        /// <summary>
        /// Sets password.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="generator">
        /// The generator.
        /// </param>
        public void SetPassword(string password, IPasswordHashGenerator generator)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(
                    "Password can not be empty.", "password");
            }

            if (password.Length < 8)
            {
                throw new ArgumentException(
                    "Password should contain at least 8 charachters.", "password");
            }

            var salt = generator.GetSalt();

            ApplyChange(new UserPasswordSet
                {
                    UserId = id,
                    PasswordHash = generator.GetPasswordHash(password, salt),
                    PasswordSalt = salt
                });
        }

        /// <summary>
        /// Verifies given password.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="generator">
        /// The generator.
        /// </param>
        /// <returns>
        /// Verification result.
        /// </returns>
        public bool LogIn(string password, IPasswordHashGenerator generator)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ApplicationException(
                    "Password must be set before verification can be done.");
            }

            return passwordHash == generator.GetPasswordHash(password, passwordSalt);
        }

        /// <summary>
        /// Logs user out.
        /// </summary>
        public void LogOut()
        {
            ApplyChange(new UserLoggedOut
                {
                    UserId = id
                });
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(UserPasswordSet @event)
        {
            passwordHash = @event.PasswordHash;
            passwordSalt = @event.PasswordSalt;
        }
    }

    public class UserLoggedOut : Event
    {
        public Guid UserId { get; set; }
    }
}
