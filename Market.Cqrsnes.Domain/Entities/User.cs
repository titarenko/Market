using System;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Utility;

namespace Market.Cqrsnes.Domain.Entities
{
    /// <summary>
    /// Represents user of the system.
    /// </summary>
    public class User : AggregateRoot,
        IChangeAcceptor<UserCreated>,
        IChangeAcceptor<UserPasswordSet>,
        IChangeAcceptor<BalanceIncreased>,
        IChangeAcceptor<MoneyReserved>,
        IChangeAcceptor<BalanceDecreased>,
        IChangeAcceptor<BalanceDecreaseFailed>
    {
        private readonly IList<Guid> pendingPurchases = new List<Guid>();

        private string passwordHash;
        private string passwordSalt;
        private double balance;
        
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
            name.ShouldNotBeEmpty("name");

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
            password.ShouldNotBeEmpty("password");

            if (password.Length < 8)
            {
                throw new ArgumentException(
                    "Password should contain at least 8 characters.", "password");
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
        /// Logs user into the system.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="generator">
        /// The generator.
        /// </param>
        public void LogIn(string password, IPasswordHashGenerator generator)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ApplicationException(
                    "Password must be set before verification can be done.");
            }

            if (passwordHash == generator.GetPasswordHash(password, passwordSalt))
            {
                ApplyChange(new UserLoggedIn
                    {
                        UserId = id
                    });
            }
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
        /// Increases balance by given amount.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        public void IncreaseBalance(double amount)
        {
            amount.ShouldBePositive("amount");

            ApplyChange(new BalanceIncreased
            {
                UserId = id,
                Amount = amount
            });
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(UserCreated @event)
        {
            id = @event.Id;
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

        public void ReserveMoney(double amount, Guid purchaseId)
        {
            if (balance >= amount)
            {
                ApplyChange(new MoneyReserved
                    {
                        UserId = id,
                        Amount = amount,
                        PurchaseId = purchaseId
                    });
            }
            else
            {
                ApplyChange(new MoneyReservationFailed
                    {
                        UserId = id,
                        Amount = amount,
                        PurchaseId = purchaseId
                    });
            }
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(BalanceIncreased @event)
        {
            balance += @event.Amount;
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(MoneyReserved @event)
        {
            balance -= @event.Amount;
            pendingPurchases.Add(@event.PurchaseId);
        }

        public void DecreaseBalance(double amount, Guid purchaseId)
        {
            if (pendingPurchases.Contains(purchaseId))
            {
                ApplyChange(new BalanceDecreased
                    {
                        UserId = id,
                        Amount = amount,
                        PurchaseId = purchaseId
                    });
            }
            else
            {
                ApplyChange(new BalanceDecreaseFailed
                    {
                        UserId = id,
                        PurchaseId = purchaseId,
                        Amount = amount
                    });
            }
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(BalanceDecreased @event)
        {
            pendingPurchases.Remove(@event.PurchaseId);
        }

        /// <summary>
        /// Performs changes caused by event.
        /// </summary>
        /// <param name="event">Event.</param>
        public void Accept(BalanceDecreaseFailed @event)
        {
            balance += @event.Amount;
        }
    }
}
