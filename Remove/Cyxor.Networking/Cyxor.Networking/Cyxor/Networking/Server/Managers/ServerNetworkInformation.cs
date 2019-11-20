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
using System.Threading;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Cyxor.Networking
{
    public partial class Server
    {
        public class ServerNetworkInformation : NodeNetworkInformation
        {
            internal override void Internal() { throw new InvalidOperationException(); }

            protected readonly Server Server;

            internal int Interval = 3000;
            internal Timer DelayedLinksTimer;

            internal ServerNetworkInformation(Node node)
               : base(node)
            {
                Server = node as Server;
                DelayedLinksTimer = new Timer(DelayedLinksTimer_Elapsed, new object(), Interval, Timeout.Infinite);
            }

#if !(UAP10_0)
            public bool ArePendingConnections
            {
                get
                {
                    if (Node.IsConnected)
                        try
                        {
                            return Server.TcpSocket.Poll(0, SelectMode.SelectRead);
                        }
                        catch { }

                    return false;
                }
            }
#endif

            //internal System.Timers.Timer DelayedLinksTimer = new System.Timers.Timer(3000);


            // TODO:
            // 1- Verificar lo del eliminar simultaneamente del acceptedLinks
            // 2- Revisar lo del Shutdown happening twice
            // TODO: NEW IDEA!
            // Hacer un timer individual por cada cliente aceptado. En caso de que el cliente se loggee en tiempo cancelar el timer.
            void DelayedLinksTimer_Elapsed(object state)
            {
                //foreach (var pair in Server.Connections.AcceptedLinks)
                //    if ((DateTime.Now - pair.Value.Connection.LastOperationTime).TotalSeconds > Server.Config.LoginDelaySeconds)
                //    {
                //        var link = (Link)null;

                //        await Utilities.Task.Delay(15000).ConfigureAwait(false); //TODO: Just for test. Remove!

                //        if (Server.Connections.AcceptedLinks.TryRemove(pair.Key, out link))
                //            if (link.Connection.State != ConnectionState.Registering)
                //                link.Disconnect(new Result(ResultCode.ClientLoginTimeout));
                //    }

                //if (Server.IsConnected)
                //    DelayedLinksTimer.Change(Interval, Timeout.Infinite);
            }

            public override Result Validate()
            {
                return base.Validate();
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
