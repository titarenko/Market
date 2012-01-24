using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cqrsnes.Infrastructure;
using Ninject;

namespace Market.Cqrsnes.WebUi.DependencyManagement
{
    public class NinjectDependencyResolver :
        IDependencyResolver,
        System.Web.Mvc.IDependencyResolver
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

        public object GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ResolveMultiple(serviceType).Cast<object>();
        }
    }
}
