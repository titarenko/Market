namespace Market.Cqrsnes.Domain.Utility
{
    /// <summary>
    /// Defines infrastructure for hashing of passwords.
    /// </summary>
    public interface IPasswordHashGenerator
    {
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
        string GetPasswordHash(string password, string salt);

        /// <summary>
        /// Returns random salt.
        /// </summary>
        /// <returns>
        /// Random salt.
        /// </returns>
        string GetSalt();
    }
}
