using System;
using System.Collections.Generic;

namespace Cqrsnes.Infrastructure
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Saves instance.
        /// </summary>
        /// <param name="instance">Instance.</param>
        /// <typeparam name="T">Type of instance.</typeparam>
        void Save<T>(T instance);

        /// <summary>
        /// Retrieves instance by its identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">Instance type.</typeparam>
        /// <returns>Instance.</returns>
        T GetById<T>(Guid id);

        /// <summary>
        /// Retrieves collection of instances.
        /// </summary>
        /// <typeparam name="T">Instance type.</typeparam>
        /// <returns>Collection of instances.</returns>
        IEnumerable<T> GetAll<T>();

        /// <summary>
        /// Returns singleton of given type.
        /// </summary>
        /// <typeparam name="T">
        /// Type of singleton.
        /// </typeparam>
        /// <returns>
        /// Singleton instance.
        /// </returns>
        T GetSingle<T>();

        /// <summary>
        /// Loads entity, performs action on it and saves back to storage.
        /// </summary>
        /// <param name="id">Identifier of entity.</param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// Type of entity.
        /// </typeparam>
        void Change<T>(Guid id, Action<T> action);
    }
}
