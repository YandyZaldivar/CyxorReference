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
using System.Threading;
using System.Collections.Concurrent;

namespace Cyxor.Networking
{
    using static Utilities.Threading;

    sealed class PacketDeliveryQueue<T>
    {
        ConcurrentQueue<T> LowQueue = new ConcurrentQueue<T>();
        ConcurrentQueue<T> HighQueue = new ConcurrentQueue<T>();
        ConcurrentQueue<T> ExclusiveQueue = new ConcurrentQueue<T>();
        ConcurrentDictionary<int, T> PartialQueue = new ConcurrentDictionary<int, T>();

        InterlockedInt Counter = new InterlockedInt();

        internal bool Add(T item)
        {
            var delivery = default(Delivery);
            var packet = (item as Packet) ?? (delivery = (item as Delivery)).Packet;

            switch (packet.Priority)
            {
                case PacketPriority.Low: LowQueue.Enqueue(item); break;
                case PacketPriority.High: HighQueue.Enqueue(item); break;
                case PacketPriority.Exclusive: ExclusiveQueue.Enqueue(item); break;

                case PacketPriority.Partial:
                {
                    var addressHash = Utilities.HashCode.GetFrom(packet.Connection.Name);
                    var senderHash = Utilities.HashCode.GetFrom(packet.RelayConnection.Name);
                    var partialId = addressHash + senderHash + packet.Id;

                    PartialQueue.AddOrUpdate(partialId, item, (key, value) =>
                    {
                        if (delivery == null)
                            (value as Packet).Dispose(); // TODO: Apply a similar delivery mechanism when replacing a packet.
                        else
                            delivery.ReplacedPartial = value as Delivery;

                        return item;
                    });

                    break;
                }
            }

            Counter.Increment();

            return true;
        }

        internal T Take()
        {
            var item = default(T);

            bool TryTake()
            {
                if (HighQueue.TryDequeue(out item))
                    return true;

                if (ExclusiveQueue.TryDequeue(out item))
                    return true;

                if (PartialQueue.TryRemove(PartialQueue.FirstOrDefault().Key, out item))
                    return true;

                if (LowQueue.TryDequeue(out item))
                    return true;

                return false;
            }

            if (!TryTake())
                SpinWait.SpinUntil(TryTake);

            Counter.Decrement();

            return item;
        }

        internal int Reset()
        {
            T item;

            while (LowQueue.TryDequeue(out item))
                continue;

            while (HighQueue.TryDequeue(out item))
                continue;

            while (ExclusiveQueue.TryDequeue(out item))
                continue;

            PartialQueue.Clear();

            var itemsCount = Counter.Value;
            Counter.Value = 0;

            return itemsCount;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
