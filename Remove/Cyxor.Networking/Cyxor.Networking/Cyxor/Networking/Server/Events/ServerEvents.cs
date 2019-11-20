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

namespace Cyxor.Networking
{
    using Events;
    using Events.Server;

    public partial class Server
    {
        public class ServerEvents : NodeEvents
        {
            Server Server;

            protected internal ServerEvents(Server server) : base(server)
            {
                Server = server;
            }

            public event EventHandler<ClientRelayingEventArgs> ClientRelaying;
            public event EventHandler<ClientConnectedEventArgs> ClientConnected;
            public event EventHandler<ClientConnectingEventArgs> ClientConnecting;
            public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
            public event EventHandler<ClientDisconnectingEventArgs> ClientDisconnecting;

            public override void RaiseEvent<TActionEventArgs>(TActionEventArgs e, bool detached = false)
            {
                if (!detached && !Node.Config.OverrideEvents)
                    return;

                switch (e.EventId)
                {
                    case ServerEventsId.ClientRelaying: RaiseEvent(ClientRelaying, e as ClientRelayingEventArgs); break;
                    case ServerEventsId.ClientConnected: RaiseEvent(ClientConnected, e as ClientConnectedEventArgs); break;
                    case ServerEventsId.ClientConnecting: RaiseEvent(ClientConnecting, e as ClientConnectingEventArgs); break;
                    case ServerEventsId.ClientDisconnected: RaiseEvent(ClientDisconnected, e as ClientDisconnectedEventArgs); break;
                    case ServerEventsId.ClientDisconnecting: RaiseEvent(ClientDisconnecting, e as ClientDisconnectingEventArgs); break;

                    default: base.RaiseEvent(e, detached); break;
                }
            }

            public override void OnEvent<TActionEventArgs>(TActionEventArgs e)
            {
                switch (e.EventId)
                {
                    case ServerEventsId.ClientRelaying: Server.OnClientRelaying(e as ClientRelayingEventArgs); break;
                    case ServerEventsId.ClientConnected: Server.OnClientConnected(e as ClientConnectedEventArgs); break;
                    case ServerEventsId.ClientConnecting: Server.OnClientConnecting(e as ClientConnectingEventArgs); break;
                    case ServerEventsId.ClientDisconnected: Server.OnClientDisconnected(e as ClientDisconnectedEventArgs); break;
                    case ServerEventsId.ClientDisconnecting: Server.OnClientDisconnecting(e as ClientDisconnectingEventArgs); break;

                    default: base.OnEvent(e); break;
                }
            }

            public override bool IsSubscribed(int eventId)
            {
                switch (eventId)
                {
                    case ServerEventsId.ClientRelaying: return ClientRelaying != null;
                    case ServerEventsId.ClientConnected: return ClientConnected != null;
                    case ServerEventsId.ClientConnecting: return ClientConnecting != null;
                    case ServerEventsId.ClientDisconnected: return ClientDisconnected != null;
                    case ServerEventsId.ClientDisconnecting: return ClientDisconnecting != null;

                    default: return base.IsSubscribed(eventId);
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
