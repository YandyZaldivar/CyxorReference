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
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cyxor.Controllers
{
    using Models;
    using Networking;
    using Networking.Events.Server;

    [Controller(Bounds = ControllerBounds.Server)]
    public class ServerController : Controller
    {
        Server server;
        protected new Server Node => server ?? (server = base.Node as Server);

        public object Stats()
            => new
            {
                Engine = "Cyxor Server v0.2.0",
                Node.Statistics.SessionTime,
                Node.Statistics.IdleTime,
                Node.Statistics.LastOperationDate,
                Node.Statistics.ConnectionsCount,
                Node.Statistics.MaxConnectionsCount,
                Node.Statistics.MaxConnectionsDate,
                Node.Statistics.AverageBytesPerSecond,
                Node.Statistics.AverageBytesSentPerSecond,
                Node.Statistics.AverageBytesReceivedPerSecond,
                Node.Statistics.SentBytes,
                Node.Statistics.ReceivedBytes,
            };

        //protected void EndPacket(Link link, Result result)
        //{
        //    if (link.DisconnectLinkReference.CompareExchange(1, 0) == 0)
        //        using (var encryptor = link.Noob.Aes.CreateEncryptor())
        //        using (var endPacket = new Packet(link) { Id = PackeId.Shutdown, Internal = true })
        //        {
        //            var buffer = new Buffer();
        //            buffer.Reset();
        //            serializer.Write((ISerializable)result);
        //            endpacket.Serializer.WriteRaw(encryptor.TransformFinalBlock(buffer.Data, 0, buffer.Length));
        //            endPacket.SendAsync();
        //        }
        //}

        void SendServerClientNames(Packet packet)
        {
            var serverConnections = new ServerConnections { Names = Node.Connections.NameList };

            using (var packetReply = new Packet(packet) { Model = serverConnections, RelayConnection = packet.Connection })
                packetReply.SendAsync();
        }

        // TODO: Add this services to the server to only use them if the server has it enabled.
        internal async Task SpreadClientService(Connection connection, ClientServices service)
        //internal void SpreadClientService(Connection connection, ClientServices service)
        {
            var connections = new List<Connection>(Node.Connections.Count);

            foreach (var item in Node.Connections.List)
                if (connection != item)
                    if ((item.Services & service) == service)
                        connections.Add(item);

            if (connections.Count > 0)
                using (var packet = new Packet(connections) { Id = (int)service, Internal = true, Model = connection.Name, RelayConnection = connection })
                    await packet.SendAsync().ConfigureAwait(false);
        }

        //[Action(InternalCoreApiId.Login, @internal: true)]
        public async Task<LoginResponse> Login(LoginRequest login)
        {
            var loginResponse = new LoginResponse();

            try
            {
                if (Connection.State != ConnectionState.Connected)
                {
                    loginResponse.Result = new Result(ResultCode.ProtocolError);
#pragma warning disable 4014
                    Connection.DisconnectAsync(loginResponse.Result);
#pragma warning restore 4014
                    return loginResponse;
                }

                if (!Node.Connections.TryRemoveFromConnectedLinks(Connection.Link))
                    return loginResponse; //new Result(ResultCode.Error, "No connection in accepted links");

                //var login = packet.GetMessage<LoginRequest>();

                if (string.IsNullOrEmpty(login.Name))
                    Connection.Name = "CY" + Math.Abs(Connection.Link.AddressHash).ToString();
                else
                    Connection.Name = login.Name;

                SpinWait.SpinUntil(() => Node.Connections.TryAddToTemporalNames(Connection.Link));

                if (Node.Connections.Find(Connection.Name) != null)
                {
                    loginResponse.Result = new Result(ResultCode.AccountNameTaken);
#pragma warning disable 4014
                    Connection.DisconnectAsync(loginResponse.Result);
#pragma warning restore 4014
                    return loginResponse;
                }

                Connection.Link.TcpSocket.NoDelay = login.NoDelay;

                Connection.Services = login.Services;
                Connection.CustomData = login.CustomData;
                Connection.State = ConnectionState.Authenticating;
                //connection.UserVersion = new System.Version(login.UserVersion);
                //connection.CyxorVersion = new System.Version(login.CyxorVersion);

                var clientConnecting = new ClientConnectingEventArgs(Connection);

                await Node.Events.Post(clientConnecting).ConfigureAwait(false);

                if (clientConnecting.Cancel)
                {
                    loginResponse.Result = new Result(ResultCode.ClientConnectionCanceled, clientConnecting.CancelReason);
#pragma warning disable 4014
                    Connection.DisconnectAsync(loginResponse.Result);
#pragma warning restore 4014
                    return loginResponse;
                }

                if (!Node.Connections.TryAddToAuthenticatedLinks(Connection.Link))
                    Node.Log(LogCategory.Fatal, "");

                Connection.State = ConnectionState.Authenticated;

                Node.Statistics.AuthenticatedConnectionsIncrement();

                await SpreadClientService(Connection, ClientServices.ClientConnected);

                loginResponse.Name = Connection.Name;
                loginResponse.CustomData = clientConnecting.CustomData;

                if (Node.Config.UdpEnabled)
                    loginResponse.UdpKey = login.UdpEnabled ? Guid.NewGuid() : Guid.Empty;

                //Connection.LoginResponse = loginResponse;

                //using (var packetReply = new Packet(packet) { Message = loginResponse })
                //{
                //    if (!Node.Config.UdpEnabled)
                //        await packetReply.SendAsync().ConfigureAwait(false);
                //    else
                //    {
                //        Connection.Link.Receives.SocketReceiveAsync(Node.UdpSocket);

                //        if (!await packetReply.QueryAsync().ConfigureAwait(false))
                //        {
                //            await Connection.DisconnectAsync(packetReply.Result.Value).ConfigureAwait(false);
                //            return;
                //        }

                //        var udpHello = packetReply.Result.Reply.GetMessage<UdpHello>();

                //        if (udpHello.Key != loginResponse.UdpKey)
                //        {
                //            await Connection.DisconnectAsync(packetReply.Result.Value).ConfigureAwait(false);
                //            return;
                //        }

                //        using (var udpReply = new Packet(packetReply) { Message = new UdpHello() })
                //            await udpReply.SendAsync().ConfigureAwait(false);

                //        Connection.Link.RemoteUDPEndPoint = Node.UdpSocket.RemoteEndPoint as IPEndPoint;
                          Connection.Link.RemoteUDPEndPoint = null;
                //    }
                //}

                Node.Log(LogCategory.ClientIn, $"[{DateTime.Now.ToString("HH:mm:ss")}] <{loginResponse.Name}> connected");
                Node.Events.Post(new ClientConnectedEventArgs(Connection));

                return loginResponse;
            }
            catch (Exception ex)
            {
                Node.Log(LogCategory.Fatal, ex);
                return loginResponse;
            }
            finally
            {
                Node.Connections.TryRemoveFromTemporalNames(Connection.Link);
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
