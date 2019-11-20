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
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Cyxor.Networking
{
    public partial class Server
    {
        public class ServerConnections : ServerProperty
        {
            ConcurrentDictionary<int, Link> ConnectedLinks { get; } = new ConcurrentDictionary<int, Link>();
            ConcurrentDictionary<int, Link> TemporalNames { get; } = new ConcurrentDictionary<int, Link>();
            ConcurrentDictionary<int, Link> AuthenticatedLinks { get; } = new ConcurrentDictionary<int, Link>();
            ConcurrentDictionary<int, string> ConnectionNames { get; } = new ConcurrentDictionary<int, string>();

            protected internal ServerConnections(Server server) : base(server) { }

            internal IEnumerable<Link> Links => AuthenticatedLinks.Select(item => item.Value);
            Link GetLink(string clientName) => GetLink(Utilities.HashCode.GetFrom(clientName));
            Link GetLink(int hashCode) => AuthenticatedLinks.SingleOrDefault(p => p.Key == hashCode).Value;

            public int Count => Server.Statistics.ConnectionsCount;
            public Connection Find(int hashCode) => GetLink(hashCode)?.Connection;
            public Connection Find(string clientName) => GetLink(clientName)?.Connection;
            public IEnumerable<Connection> List => from link in Links select link.Connection;
            public IEnumerable<string> NameList => ConnectionNames.Select(item => item.Value);
            public string GetName(int hashCode) => ConnectionNames.SingleOrDefault(p => p.Key == hashCode).Value;

            internal bool Exists(Link link) => AuthenticatedLinks.TryGetValue(link.GetHashCode(), out link);

            internal bool TryAddToConnectedLinks(Link link) => ConnectedLinks.TryAdd(link.AddressHash, link);
            internal bool TryAddToTemporalNames(Link link) => TemporalNames.TryAdd(link.GetHashCode(), link);
            internal bool TryAddToAuthenticatedLinks(Link link) => AuthenticatedLinks.TryAdd(link.GetHashCode(), link);

            internal bool TryRemoveFromConnectedLinks(Link link) => ConnectedLinks.TryRemove(link.AddressHash, out link);
            internal bool TryRemoveFromTemporalNames(Link link) => TemporalNames.TryRemove(link.GetHashCode(), out link);
            internal bool TryRemoveFromAuthenticatedLinks(Link link) => AuthenticatedLinks.TryRemove(link.GetHashCode(), out link);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
