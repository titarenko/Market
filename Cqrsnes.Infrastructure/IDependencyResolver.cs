using System;
using System.Collections;
using System.Collections.Generic;

namespace Cqrsnes.Infrastructure
{
    public interface IDependencyResolver
    {
        IEnumerable ResolveMultiple(Type type);
        IEnumerable<T> ResolveMultiple<T>();

        object Resolve(Type type);
        T Resolve<T>();
    }
}