using ServiceStack.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StructureMap;

namespace eShop
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        private readonly IContainer _container;

        public StructureMapContainerAdapter(IContainer container)
        {
            _container = container;
        }

        public T TryResolve<T>()
        {
            return _container.TryGetInstance<T>();
        }

        public T Resolve<T>()
        {
            var ret = _container.TryGetInstance<T>();
            if (ret == null) throw new ArgumentException($"Unknown resolution type '{typeof(T)}'");
            return ret;
        }
    }
}