using System;
using System.Security.Cryptography;
using System.Text;

namespace Market.Cqrsnes.Domain.Utility
{
    /// <summary>
    /// Provides infrastructure for hashing of passwords.
    /// </summary>
    public class PasswordHashGenerator : IPasswordHashGenerator
    {
        private readonly MD5 md5;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHashGenerator"/> class.
        /// </summary>
        public PasswordHashGenerator()
        {
            md5 = MD5.Create();
        }

        /// <summary>
        /// Returns hash for given password and salt.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="salt">
        /// The salt.
        /// </param>
        /// <returns>
        /// Hash.
        /// </returns>
        public string GetPasswordHash(string password, string salt)
        {
            return GetHash(GetHash(password) + salt);
        }

        /// <summary>
        /// Returns random salt.
        /// </summary>
        /// <returns>
        /// Random salt.
        /// </returns>
        public string GetSalt()
        {
            return Guid.NewGuid().ToString();
        }

        private string GetHash(string text)
        {
            return BitConverter.ToString(
                md5.ComputeHash(Encoding.Default.GetBytes(text)));
        }
    }
}
