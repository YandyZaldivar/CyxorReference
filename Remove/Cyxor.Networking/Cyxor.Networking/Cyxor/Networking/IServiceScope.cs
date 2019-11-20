/*
  { Cyxor } - Core Networking Communications <http://www.cyxor.com/>
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
using System.Collections.Generic;

namespace Cyxor.Networking
{
    /// <summary>
    /// The <see cref="IDisposable.Dispose"/> method ends the scope lifetime. Once Dispose is called,
    /// any scoped services that have been resolved will be disposed.
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// The <see cref="IServiceProvider"/> used to resolve dependencies from the scope.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        T GetService<T>(bool allowSubclasses = true) where T : class;

        object GetService(Type serviceType, bool allowSubclasses = true);

        IEnumerable<T> GetServices<T>(bool allowSubclasses = true) where T : class;

        IEnumerable<object> GetServices(Type serviceType, bool allowSubclasses = true);
    }
}
/* { Cyxor } - Core Networking Communications <http://www.cyxor.com/> */
