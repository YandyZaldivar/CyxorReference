/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace Cyxor.Networking
{
    public struct ServiceScope : IServiceScope, Microsoft.Extensions.DependencyInjection.IServiceScope
    {
        public IServiceCollection Services { get; }
        public Microsoft.Extensions.DependencyInjection.IServiceScope Scope { get; }

        public IServiceProvider ServiceProvider => Scope.ServiceProvider;

        public ServiceScope(Microsoft.Extensions.DependencyInjection.IServiceScope scope, IServiceCollection services)
        {
            Scope = scope;
            Services = services;
        }

        public object GetService(Type serviceType, bool allowSubclasses = true)
        {
            var service = ServiceProvider.GetService(serviceType);

            if (service == null && allowSubclasses)
            {
                var descriptor = default(ServiceDescriptor);

                if (serviceType.GetTypeInfo().IsClass)
                    descriptor = Services.FirstOrDefault(p => p.ServiceType.GetTypeInfo().IsSubclassOf(serviceType));
                else if (serviceType.GetTypeInfo().IsInterface)
                    descriptor = Services.FirstOrDefault(p => p.ServiceType.GetTypeInfo().ImplementedInterfaces.Contains(serviceType));

                if (descriptor != null)
                    service = ServiceProvider.GetService(descriptor.ServiceType);
            }

            return service;
        }

        public T GetService<T>(bool allowSubclasses = true) where T : class => GetService(typeof(T), allowSubclasses) as T;

        public IEnumerable<object> GetServices(Type serviceType, bool allowSubclasses = true)
        {
            var services = new List<object>(ServiceProvider.GetServices(serviceType));

            if (allowSubclasses)
            {
                if (serviceType.GetTypeInfo().IsClass)
                    foreach (var descriptor in Services.Where(p => p.ServiceType.GetTypeInfo().IsSubclassOf(serviceType)))
                        services.Add(ServiceProvider.GetService(descriptor.ServiceType));
                else
                    foreach (var descriptor in Services.Where(p => p.ServiceType.GetTypeInfo().ImplementedInterfaces.Contains(serviceType)))
                        services.Add(ServiceProvider.GetService(descriptor.ServiceType));
            }

            return services;
        }

        public IEnumerable<T> GetServices<T>(bool allowSubclasses = true) where T : class
        {
            foreach (var service in GetServices(typeof(T), allowSubclasses))
                yield return service as T;
        }

        public void Dispose() => Scope.Dispose();
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
