using System;
using System.Collections.Generic;
using ServiceStack.Redis;

namespace Cqrsnes.Infrastructure.Impl
{
    public class RedisRepository : IRepository
    {
        private readonly IRedisClient client;

        public RedisRepository(IRedisClient client)
        {
            this.client = client;
        }

        public void Save<T>(T instance)
        {
            using (var collection = client.GetTypedClient<T>())
            {
                collection.Store(instance);
                collection.Save();
            }
        }

        public T GetById<T>(Guid id)
        {
            using (var collection = client.GetTypedClient<T>())
            {
                return collection.GetById(id);
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            using (var collection = client.GetTypedClient<T>())
            {
                return collection.GetAll();
            }
        }
    }
}