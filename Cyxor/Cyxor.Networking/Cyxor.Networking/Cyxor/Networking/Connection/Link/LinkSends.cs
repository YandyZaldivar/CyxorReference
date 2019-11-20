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
    using static Utilities.Threading;

    sealed partial class Link
    {
        internal sealed class LinkSends : LinkProperty
        {
            InterlockedCountdown Countdown;
            InterlockedInt QueueReferences;

            PacketDeliveryQueue<Delivery> Queue;

            internal LinkSends(Link link) : base(link)
            {
                Queue = new PacketDeliveryQueue<Delivery>();
                Countdown = new InterlockedCountdown(initialCount: 0);
            }

            internal int AcquireQueue()
                => QueueReferences.Increment();

            internal int ReleaseQueue()
                => QueueReferences.Decrement();

            internal bool AddDelivery(Delivery delivery)
                => Queue.Add(delivery);

            internal Delivery TakeDelivery()
                => Queue.Take();

            internal void Initialize()
                => Countdown.Reset(initialCount: 1);

            internal bool Acquire()
            {
                var references = Countdown.Acquire();

                if (references <= 0)
                    return false;
                else if (references == int.MaxValue)
                    Node.Log(LogCategory.Fatal, $"{nameof(Link)} send references max value reached.");

                return true;
            }

            internal void Release()
            {
                var references = Countdown.Release();

                if (references == 0)
                    Link.DisconnectReferencesIncrement();
                else if (references < 0)
                    Node.Log(LogCategory.Fatal, $"{nameof(Link)} send references decrement below zero.");
            }

            internal void Reset()
            {
                var pendingPackets = Queue.Reset();

                if (pendingPackets != 0)
                    Node.Log(LogCategory.Warning, $"{nameof(Link)} reset with pending packets ({pendingPackets}) in the output queue.");

                var referenceCountCheck = 0;

                referenceCountCheck |= Countdown.Count;
                referenceCountCheck |= QueueReferences.Value;

                if (referenceCountCheck != 0)
                    Link.Node.Log(LogCategory.Fatal, $"{nameof(Link)} reset with invalid send references count.");

                Countdown.Reset();
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
