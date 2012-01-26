using System;
using System.Collections.Generic;
using System.Linq;
using Cqrsnes.Infrastructure;

namespace Cqrsnes.Test
{
    /// <summary>
    /// Repository for testing purposes (leaves entities transient).
    /// </summary>
    public class TestRepository : IRepository
    {
        private readonly IDictionary<string, object> objects = new Dictionary<string, object>();

        /// <summary>
        /// Saves instance.
        /// </summary>
        /// <param name="instance">Instance.</param>
        /// <typeparam name="T">Type of instance.</typeparam>
        public void Save<T>(T instance)
        {
            var id = (Guid) typeof(T).GetProperty("Id").GetValue(instance, null);
            var key = GetKey<T>(id);
            if (objects.ContainsKey(key))
            {
                objects[key] = instance;
            }
            else
            {
                objects.Add(key, instance);
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
            var key = GetKey<T>(id);
            if (objects.ContainsKey(key))
            {
                return (T) objects[key];
            }
            return Activator.CreateInstance<T>();
        }

        private static string GetKey<T>(Guid id)
        {
            return typeof(T).FullName + id;
        }

        /// <summary>
        /// Retrieves collection of instances.
        /// </summary>
        /// <typeparam name="T">Instance type.</typeparam>
        /// <returns>Collection of instances.</returns>
        public IEnumerable<T> GetAll<T>()
        {
            return objects.Values.OfType<T>();
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
            return objects.Values.OfType<T>().First();
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
