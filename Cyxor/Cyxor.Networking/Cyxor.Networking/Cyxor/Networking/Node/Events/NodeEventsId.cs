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
    public abstract partial class Node
    {
        public class NodeEventsId
        {
            internal const int MessageLogged = 0;

            internal const int SslCertificateSelecting = 1;
            internal const int SslCertificateValidating = 2;

            internal const int CommandExecuting = 10;
            internal const int CommandExecuteCompleted = 11;

            internal const int Disconnecting = 20;
            internal const int ConnectCompleted = 21;
            internal const int DisconnectCompleted = 22;
            internal const int ConnectProgressChanged = 23;

            internal const int PacketSendCompleted = 30;
            internal const int PacketReceiveCompleted = 31;
            internal const int PacketSendProgressChanged = 32;
            internal const int PacketReceiveProgressChanged = 33;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
