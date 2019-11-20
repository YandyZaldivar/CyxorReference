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
    using Events.Client;

    public partial class Client
    {
        public class ClientEvents : NodeEvents
        {
            Client Client;

            protected internal ClientEvents(Client client) : base(client)
            {
                Client = client;
            }

            public event EventHandler<ClientConnectedEventArgs> ClientConnected;
            public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

            public override void RaiseEvent<TActionEventArgs>(TActionEventArgs e, bool detached = false)
            {
                if (!detached && !Node.Config.OverrideEvents)
                    return;

                switch (e.EventId)
                {
                    case ClientEventsId.ClientConnected: RaiseEvent(ClientConnected, e as ClientConnectedEventArgs); break;
                    case ClientEventsId.ClientDisconnected: RaiseEvent(ClientDisconnected, e as ClientDisconnectedEventArgs); break;

                    default: base.RaiseEvent(e, detached); break;
                }
            }

            public override void OnEvent<TActionEventArgs>(TActionEventArgs e)
            {
                switch (e.EventId)
                {
                    case ClientEventsId.ClientConnected: Client.OnClientConnected(e as ClientConnectedEventArgs); break;
                    case ClientEventsId.ClientDisconnected: Client.OnClientDisconnected(e as ClientDisconnectedEventArgs); break;

                    default: base.OnEvent(e); break;
                }
            }

            public override bool IsSubscribed(int eventType)
            {
                switch (eventType)
                {
                    case ClientEventsId.ClientConnected: return ClientConnected != null;
                    case ClientEventsId.ClientDisconnected: return ClientDisconnected != null;

                    default: return base.IsSubscribed(eventType);
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
