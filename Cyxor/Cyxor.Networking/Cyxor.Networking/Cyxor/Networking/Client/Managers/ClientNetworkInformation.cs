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

namespace Cyxor.Networking
{
    public partial class Client
    {
        public class ClientNetworkInformation : NodeNetworkInformation
        {
            internal override void Internal() { throw new InvalidOperationException(); }

            internal ClientNetworkInformation(Node node) : base(node) { }

            public bool IsUDPEnabled { get; internal set; }

            public string RemoteAddress => Node.Config.Address;
            public string RemoteHostName => Node.IPHostEntry?.HostName;
            public EndPoint RemoteEndPoint => (Node as Client).RemoteEndPoint;
            public string RemoteIpAddress => ((Node as Client).RemoteEndPoint as IPEndPoint)?.Address?.ToString();
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
