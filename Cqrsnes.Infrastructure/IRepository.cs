using System;
using System.Collections.Generic;

namespace Cqrsnes.Infrastructure
{
    public interface IRepository
    {
        void Save<T>(T instance);
        T GetById<T>(Guid id);
        IEnumerable<T> GetAll<T>();
    }
}