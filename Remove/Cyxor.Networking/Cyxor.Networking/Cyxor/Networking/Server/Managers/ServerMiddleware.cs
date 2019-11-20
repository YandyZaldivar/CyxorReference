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
    public partial class Server
    {
        public class ServerMiddleware : NodeMiddleware
        {
            protected Server Server { get; }

            protected internal ServerMiddleware(Server server) : base(server) { Server = server; }

            protected internal override void OnPacketReceived(Connection connection, Packet packet)
            {
                if (packet.IsToNode)
                    base.OnPacketReceived(connection, packet);
                else
                {
                    new Action(async () => // TODO: Terminar
                    {
                        using (packet)
                        {
                            var relaying = Node.Events.Post(new Events.Server.ClientRelayingEventArgs(packet));
                            await relaying.ConfigureAwait(false);

                            //Node.Log(LogCategory.Information, $"Packet QueryMode: {packet.QueryMode.ToString()} Name {packet.RootConnection.Name}");

                            var result = await packet.SendAsync();

                            if (!result)
                                using (var relayPacket = new Packet(packet) { Id = -1, Model = result })
                                    await relayPacket.SendAsync();
                        }
                    }).Invoke();
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
