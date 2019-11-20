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
using System.Collections.Concurrent;

namespace Cyxor.Networking.Events
{
    public abstract class ActionEventArgs : EventArgs
    {
        public Node Node { get; }

        public abstract int EventId { get; }

        internal static ConcurrentDictionary<int, bool> Overrides = new ConcurrentDictionary<int, bool>();

        // TODO: Improve this method
        public bool IsOverridden()
        {
            var value = default(bool);
            if (!Overrides.TryGetValue(EventId, out value))
            {
                var name = "On" + GetType().Name.Substring(0, length: GetType().Name.IndexOf(nameof(EventArgs)));

                var declaringType = Node.GetType()

#if NET35 || NET40
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
#else
                .GetRuntimeMethods()
#endif
                .Single(p => p.Name == name && p.GetParameters().Single().ParameterType == GetType()).DeclaringType;

                var dType = Node is Networking.Client ? typeof(Networking.Client) : typeof(Networking.Server);

                Overrides.TryAdd(EventId, declaringType != typeof(Node) && declaringType != dType);
            }

            return Overrides[EventId];
        }

        protected ActionEventArgs(Node node)
        {
            Node = node;
        }

        internal virtual void Action()
        {
            // TODO: Review the raiseEvent, para tratar de elimar
            Node.Events.RaiseEvent(this, detached: true);
            
            //GetType().GetMethod("", System.Reflection.BindingFlags.)

            if (IsOverridden())
                Node.Events.OnEvent(this);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
