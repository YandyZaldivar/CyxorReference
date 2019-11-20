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

namespace Cyxor.Networking.Config
{
    // TODO: Finish remarks.

    /// <summary>
    /// Specify how network events are presented to users.
    /// </summary>
    /// <remarks>
    /// When <see cref="Manual"/> is set, network events are not
    /// raised until explicitly invoking them by <see cref="Node.Events.Process()"/>. <see cref="Synchronized"/>
    /// events are when the user has configured a <see cref="Threading.SynchronizationContext"/> via
    /// <see cref="Node.Config.SynchronizationContext"/>, then network events are redirected to that context.
    /// <see cref="Sequenced"/>...
    /// </remarks>
    public enum EventDispatching
    {
        Concurrent,
        Sequenced,
        Synchronized,
        Manual,
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
