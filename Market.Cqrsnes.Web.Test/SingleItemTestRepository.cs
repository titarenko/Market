using System;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Web.Test
{
    public class SingleItemTestRepository : IRepository
    {
        private object instance;

        public void Save<T>(T instance)
        {
            this.instance = instance;
        }

        public T GetById<T>(Guid id)
        {
            return (T) instance;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return new[] {(T) instance};
        }
    }
}