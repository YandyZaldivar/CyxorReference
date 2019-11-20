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
    using static Utilities.Threading;

    public partial class Server
    {
        public class ServerStatistics : NodeStatistics
        {
            internal override void Internal() => new NotImplementedException();

            #region Connections

            InterlockedInt connectionsCount;
            public int ConnectionsCount => connectionsCount.Value;

            volatile int maxConnectionsCount;
            public int MaxConnectionsCount => maxConnectionsCount;

            InterlockedLong maxConnectionsDateTicks;
            public DateTime MaxConnectionsDate
            {
                get => DateTime.FromBinary(maxConnectionsDateTicks.Value);
                private set => maxConnectionsDateTicks.Value = value.ToBinary();
            }

            internal void ConnectionsIncrement()
            {
                var count = connectionsCount.Increment();

                while (count > maxConnectionsCount)
                {
                    maxConnectionsCount = count;
                    MaxConnectionsDate = DateTime.Now;
                }
            }

            internal void ConnectionsDecrement() => connectionsCount.Decrement();

            #endregion

            #region AuthenticatedConnections

            InterlockedInt authenticatedConnectionsCount;
            public int AuthenticatedConnectionsCount => authenticatedConnectionsCount.Value;

            volatile int maxAuthenticatedConnectionsCount;
            public int MaxAuthenticatedConnectionsCount => maxAuthenticatedConnectionsCount;

            InterlockedLong maxAuthenticatedConnectionsDateTicks;
            public DateTime MaxAuthenticatedConnectionsDate
            {
                get => new DateTime(maxAuthenticatedConnectionsDateTicks.Value);
                private set => maxAuthenticatedConnectionsDateTicks.Value = value.Ticks;
            }

            internal void AuthenticatedConnectionsIncrement()
            {
                var count = authenticatedConnectionsCount.Increment();

                while (count > maxAuthenticatedConnectionsCount)
                {
                    maxAuthenticatedConnectionsCount = count;
                    MaxAuthenticatedConnectionsDate = DateTime.Now;
                }
            }

            internal void AuthenticatedConnectionsDecrement() => authenticatedConnectionsCount.Decrement();

            #endregion
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
