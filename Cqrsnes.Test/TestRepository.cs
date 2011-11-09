﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cqrsnes.Infrastructure;

namespace Cqrsnes.Test
{
    public class TestRepository : IRepository
    {
        private readonly IDictionary<Guid, object> objects = new Dictionary<Guid, object>();

        public void Save<T>(T instance)
        {
            var id = (Guid) typeof (T).GetProperty("Id").GetValue(instance, null);
            if (objects.ContainsKey(id))
            {
                objects[id] = instance;
            }
            else
            {
                objects.Add(id, instance);
            }
        }

        public T GetById<T>(Guid id)
        {
            if (objects.ContainsKey(id))
            {
                return (T) objects[id];
            }
            return default(T);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return objects.Values.Cast<T>();
        }
    }
}