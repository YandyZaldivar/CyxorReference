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

using System.Threading.Tasks;

namespace Cyxor.Networking
{
    using Config;
    using Models;

    using static Utilities.Threading;

    public abstract partial class Node
    {
        public abstract class NodeMiddleware : NodeProperty
        {
            InterlockedInt PacketQueueReferences;
            PacketDeliveryQueue<Packet> PacketQueue;

            protected NodeMiddleware(Node node) : base(node)
            {
                PacketQueueReferences = new InterlockedInt();
                PacketQueue = new PacketDeliveryQueue<Packet>();
            }

            protected internal virtual void OnPacketReceived(Connection connection, Packet packet)
            {
                connection.TryAcquireReceives(packet);

                var queuedReferencesCount = 0;
                packet.Sending = PacketSending.Overlapped;

                if (Node.Config.EventDispatching != EventDispatching.Concurrent)
                {
                    packet.Sending = PacketSending.Queued;

                    ref InterlockedInt packetQueueReferences = ref Node.Config.EventDispatching == EventDispatching.Synchronized
                        ? ref PacketQueueReferences : ref connection.Link.Receives.PacketQueueReferences;

                    var packetQueue = Node.Config.EventDispatching == EventDispatching.Synchronized
                        ? PacketQueue : connection.Link.Receives.PacketQueue;

                    queuedReferencesCount = packetQueueReferences.Increment();

                    if (queuedReferencesCount > 1 || Node.Config.EventDispatching == EventDispatching.Manual)
                        packetQueue.Add(packet);
                }

                if (Node.Config.EventDispatching == EventDispatching.Concurrent || queuedReferencesCount == 1)
                    ProcessPacket(packet);
            }

            async void ProcessPacket(Packet queuedPacket)
            {
                var connection = queuedPacket.RootConnection;

                using (queuedPacket)
                {
                    using (connection.Scope = Node.CreateScope())
                    {
                        Node.Context = new Context
                        {
                            Packet = queuedPacket,
                            HttpRequest = queuedPacket.Box.HttpRequest,
                            JsonRequest = queuedPacket.Box.HttpRequest?.Body,
                        };

                        connection.Context = Node.Context;
                        await Node.Events.Post(new Events.PacketReceiveCompletedEventArgs(queuedPacket));
                        connection.Context = null;
                        Node.Context = null;
                    }

                    connection.ReleaseReceives(queuedPacket);

                    if (queuedPacket.Sending == PacketSending.Queued)
                        ProcessNextPacket();
                }

                void ProcessNextPacket()
                {
                    ref InterlockedInt packetQueueReferences = ref Node.Config.EventDispatching == EventDispatching.Synchronized
                        ? ref PacketQueueReferences : ref connection.Link.Receives.PacketQueueReferences;

                    var packetQueue = Node.Config.EventDispatching == EventDispatching.Synchronized
                        ? PacketQueue : connection.Link.Receives.PacketQueue;

                    if (packetQueueReferences.Decrement() > 0)
                        // NOTE: Run in a task to avoid deep recursion which may leads to stack overflow
                        Utilities.Task.Run(() => ProcessPacket(packetQueue.Take())).ConfigureAwait(false);
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
