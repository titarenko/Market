using System;

namespace Cqrsnes.Infrastructure
{
    public interface IAggregateRootRepository
    {
        void Save<T>(T instance) where T : AggregateRoot, new();
        T GetById<T>(Guid id) where T : AggregateRoot, new();
    }
}