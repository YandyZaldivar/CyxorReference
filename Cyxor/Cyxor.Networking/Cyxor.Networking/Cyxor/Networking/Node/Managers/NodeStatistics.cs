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

    public partial class Node
    {
        public abstract class NodeStatistics
        {
            internal abstract void Internal();

            InterlockedLong sentBytes;
            public long SentBytes => sentBytes.Value;
            internal void AddSentBytes(int bytes) => sentBytes.Add(bytes);

            InterlockedLong receivedBytes;
            public long ReceivedBytes => receivedBytes.Value;
            internal void AddReceivedBytes(int bytes) => receivedBytes.Add(bytes);

            public long AverageBytesSentPerSecond => SessionTime == TimeSpan.Zero ? 0 : (long)(sentBytes.Value / SessionTime.TotalSeconds);
            public long AverageBytesReceivedPerSecond => SessionTime == TimeSpan.Zero ? 0 : (long)(receivedBytes.Value / SessionTime.TotalSeconds);
            public long AverageBytesPerSecond => SessionTime == TimeSpan.Zero ? 0 : (long)((sentBytes.Value + receivedBytes.Value) / SessionTime.TotalSeconds);

            public DateTime? ConnectionDate { get; internal set; }
            public DateTime? DisconnectionDate { get; internal set; }
            public DateTime? LastOperationDate { get; internal set; }
            public TimeSpan IdleTime => LastOperationDate != null ? (DateTime.Now - LastOperationDate).Value : TimeSpan.Zero;
            public TimeSpan SessionTime
            {
                get
                {
                    if (ConnectionDate == null)
                        return TimeSpan.Zero;

                    if (DisconnectionDate == null)
                        return (DateTime.Now - ConnectionDate).Value;
                    else
                        return (DisconnectionDate - ConnectionDate).Value;
                }
            }

            protected internal virtual void Reset()
            {
                sentBytes.Exchange(0);
                receivedBytes.Exchange(0);

                ConnectionDate = null;
                DisconnectionDate = null;
                LastOperationDate = null;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
