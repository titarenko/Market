using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;

namespace Cqrsnes.Infrastructure.Impl
{
    /// <summary>
    /// Repository based on Redis key/value storage.
    /// </summary>
    public class RedisRepository : IRepository
    {
        private readonly IRedisClient client;

        /// <summary>
        /// Constructs new instance.
        /// </summary>
        /// <param name="client">Redis client.</param>
        public RedisRepository(IRedisClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Saves instance.
        /// </summary>
        /// <param name="instance">Instance.</param>
        /// <typeparam name="T">Type of instance.</typeparam>
        public void Save<T>(T instance)
        {
            using (var collection = client.GetTypedClient<T>())
            {
                collection.Store(instance);
                collection.Save();
            }
        }

        /// <summary>
        /// Retrieves instance by its identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">Instance type.</typeparam>
        /// <returns>Instance.</returns>
        public T GetById<T>(Guid id)
        {
            using (var collection = client.GetTypedClient<T>())
            {
                return collection.GetById(id);
            }
        }

        /// <summary>
        /// Retrieves collection of instances.
        /// </summary>
        /// <typeparam name="T">Instance type.</typeparam>
        /// <returns>Collection of instances.</returns>
        public IEnumerable<T> GetAll<T>()
        {
            using (var collection = client.GetTypedClient<T>())
            {
                return collection.GetAll();
            }
        }

        /// <summary>
        /// Returns singleton of given type.
        /// </summary>
        /// <typeparam name="T">
        /// Type of singleton.
        /// </typeparam>
        /// <returns>
        /// Singleton instance.
        /// </returns>
        public T GetSingle<T>()
        {
            using (var collection = client.GetTypedClient<T>())
            {
                // TODO: verify
                return collection.GetEarliestFromRecentsList(0, 1).FirstOrDefault();
            }
        }

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
        public void Change<T>(Guid id, Action<T> action)
        {
            var instance = GetById<T>(id);
            action(instance);
            Save(instance);
        }
    }
}
