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
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Cyxor.Networking
{
    using Serialization;

    using static Utilities.Threading;

    public class Context : IDisposable
    {
        public Result? Result { get; set; }
        public Packet Packet { get; set; }
        public string JsonRequest { get; set; }
        public HttpRequest HttpRequest { get; set; }

        public void Dispose()
        {
            Result = null;
            Packet = null;
            HttpRequest = null;
            JsonRequest = null;
        }
    }

    public partial class Connection
    {
        public Node Node => Link?.Node;

        public Connection() : this(link: null) { }

        public Context Context { get; set; }

        protected int HashCode { get; set; }
        protected bool Disposed { get; set; }

        public virtual int Security { get; protected set; }
        public IEnumerable<string> Roles { get; protected set; }
        public virtual string Name { get; protected internal set; }

        protected internal virtual IServiceScope Scope { get; set; }
        public ConnectionStatistics Statistics { get; protected set; }
        public ConnectionNetworkInformation NetworkInformation { get; protected set; }

        public bool IsAuthenticated { get; set; }
        public bool? IsHttp { get; internal set; }
        public Result Result { get; internal set; }
        public bool UdpEnabled { get; internal set; }
        public bool FirstLogin { get; internal set; }
        public Serializer CustomData { get; internal set; }
        public ClientServices Services { get; internal set; }
        public IPEndPoint RemoteEndPoint { get; internal set; }
        //public string DisconnectionReason { get; internal set; }
        public int LastPingTimeMilliseconds { get; internal set; }
        public ConnectionState State { get; protected internal set; }
        public global::System.Version UserVersion { get; internal set; }
        public global::System.Version CyxorVersion { get; internal set; }
        public DisconnectionSource DisconnectionSource { get; internal set; }
        public bool Active => Link == null ? false : Link.TcpSocket?.Connected ?? false;

        public object Tag { get; set; }
        public Dictionary<string, object> Tags { get; set; }

        internal Connection(Link link)
        {
            Link = link;

            Tags = new Dictionary<string, object>();
            Statistics = new ConnectionStatistics();
            NetworkInformation = new ConnectionNetworkInformation(this);
        }

        Link link;
        internal Link Link
        {
            get
            {
                if (Disposed)
                    throw new InvalidOperationException("This connection object has been disposed.");

                return link;
            }
            set
            {
                if (Disposed)
                    throw new InvalidOperationException("This connection object has been disposed.");

                link = value;
            }
        }

        internal bool TryAcquireSends() => link?.Sends.Acquire() ?? false;
        internal bool TryAcquireReceives(Socket socket) => link?.Receives.Acquire(socket) ?? false;
        internal bool TryAcquireReceives(Packet packet) => link?.Receives.Acquire(packet) ?? false;

        internal void ReleaseSends() => Link.Sends.Release();
        internal void ReleaseReceives(Socket socket) => Link.Receives.Release(socket);
        internal void ReleaseReceives(Packet packet) => Link.Receives.Release(packet);

        internal void ConnectAsync(Link link)
        {
            if (Link != null)
                if (Link != link)
                    Node.Log(LogCategory.Fatal, "Link objects mismatch in connection initialization.");

            Link = link;

            Reset();
        }

        public IAwaiter DisconnectAsync(Result result = default, ShutdownSequence shutdownSequence = ShutdownSequence.Graceful, DisconnectionSource disconnectionSource = DisconnectionSource.None)
        {
            if (Node.IsClient)
            {
                Node.AsClient.DisconnectAsync(result, shutdownSequence, disconnectionSource);
                return Node;
            }
            else
                return Link.DisconnectAsync(result, shutdownSequence, disconnectionSource);
        }

        //public IAwaiter DisconnectAsync(Result result = default, ShutdownSequence shutdownSequence = ShutdownSequence.Graceful)
        //    => DisconnectAsync(result, shutdownSequence, DisconnectionSource.None);

        public override int GetHashCode() => HashCode == 0 ? HashCode = Utilities.HashCode.GetFrom(Name) : HashCode;

        protected virtual void Reset()
        {
            Name = null;
            HashCode = 0;
            Security = 0;
            UdpEnabled = false;
            FirstLogin = false;
            UserVersion = null;
            Statistics.Reset();
            CyxorVersion = null;
            RemoteEndPoint = null;
            Result = Result.Success;
            LastPingTimeMilliseconds = -1;
            Services = ClientServices.None;
            //DisconnectionReason = default;
            DisconnectionSource = DisconnectionSource.None;

            if (CustomData != null)
            {
                Node?.Pools.PushBuffer(CustomData);
                CustomData = null;
            }
        }

        protected internal virtual void Dispose()
        {
            if (!Disposed)
            {
                link = null;
                RemoteEndPoint = null;
                State = ConnectionState.Disconnected;

                Disposed = true;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
