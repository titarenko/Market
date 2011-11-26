using System;
using System.Collections;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Ninject;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IEnumerable ResolveMultiple(Type type)
        {
            return kernel.GetAll(type);
        }

        public IEnumerable<T> ResolveMultiple<T>()
        {
            return kernel.GetAll<T>();
        }

        public object Resolve(Type type)
        {
            return kernel.Get(type);
        }

        public T Resolve<T>()
        {
            return kernel.Get<T>();
        }
    }
}