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
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Cyxor.Networking
{
    using Models;
    using Security;
    using Config.Client;
    using Networking.Config;

    public partial class Client : Node
    {
        internal override void Internal() => throw new InvalidOperationException();

        //internal TaskCompletionSource<Result> ConnectCompletion;

        public static new Client Instance => LazyInstance.Value;
        static Lazy<Client> LazyInstance = new Lazy<Client>(() => new Client());

        //public ClientAccount Account { get; private set; }

        // TODO: Restore this
        //internal Link Link { get; private set; }
        internal Link Link { get; set; }

        public Connection Connection => Link.Connection;

        internal EndPoint RemoteEndPoint { get; set; }
        internal RandomNumberGenerator Rng { get; set; }

        internal override Socket TcpSocket
        {
            get => Link?.TcpSocket;
            set => Link.TcpSocket = value;
        }

        public new ClientConfig Config
        {
            get => base.Config as ClientConfig;
            protected set => base.Config = value;
        }

        public new ClientEvents Events
        {
            get => base.Events as ClientEvents;
            protected set => base.Events = value;
        }

        public new ClientControllers Controllers
        {
            get => base.Controllers as ClientControllers;
            protected set => base.Controllers = value;
        }

        protected internal new ClientMiddleware Middleware
        {
            get => base.Middleware as ClientMiddleware;
            protected set => base.Middleware = value;
        }

        public new Connection.ConnectionStatistics Statistics
        {
            get => base.Statistics as Connection.ConnectionStatistics;
            private set => base.Statistics = value;
        }

        public new ClientNetworkInformation NetworkInformation
        {
            get => base.NetworkInformation as ClientNetworkInformation;
            private set => base.NetworkInformation = value;
        }

        public bool IsAuthenticated => TcpSocket != null ? TcpSocket.Connected ? Connection.State == ConnectionState.Authenticated : false : false;

        public override bool IsConnected => TcpSocket != null ? TcpSocket.Connected ? Connection.State >= ConnectionState.Connected : false : false;

        public override string ToString() => string.Format(Utilities.ResourceStrings.ClientDebuggerDisplayFormat, Config.Name, Connection.State, Config.Address, Config.Port);

        public Client()
        {
            Config = new ClientConfig();

            Link = Pools.PopLink();
            Events = new ClientEvents(this);

            //Account = new ClientAccount(this);
            Statistics = Connection.Statistics;
            Rng = RandomNumberGenerator.Create();
            Middleware = new ClientMiddleware(this);
            Controllers = new ClientControllers(this);
            NetworkInformation = new ClientNetworkInformation(this);
        }

        protected internal virtual void OnClientConnected(Events.Client.ClientConnectedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnClientDisconnected(Events.Client.ClientDisconnectedEventArgs e) => Events.RaiseEvent(e);

        //internal void UpdateAddresses()
        //{
        //    var preferredAddressMethod = NodeNetworkInformation.PreferredLocalIPAddress;

        //    var ipAddress = default(IPAddress);
        //    if (Config.PreferIPv4Addresses)
        //        if (!IPAddress.TryParse(Config.Address, out ipAddress))
        //            preferredAddressMethod = NodeNetworkInformation.PreferredLocalIPv4Address;
        //        else if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
        //            preferredAddressMethod = NodeNetworkInformation.PreferredLocalIPv4Address;

        //    RemoteEndPoint = new IPEndPoint(preferredAddressMethod, Config.Port);
        //}

        public async Task<Result> ProxyConnectAsync()
        {
            var result = Result.Success;

            try
            {
                if (!Config.Proxy.Enabled)
                    return result;

                //TcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);



                //var httpVersion = default(string);

                //switch (HttpVersion)
                //{
                //    case HttpVersion.Http_1_0: httpVersion = "HTTP/1.0"; break;
                //    case HttpVersion.Http_1_1: httpVersion = "HTTP/1.1"; break;
                //}

                var proxyAuthorization = default(string);

                var endOfLine = "\r\n";
                var contentLength = 0;
                var contentLengthHeader = "Content-Length: ".ToLowerInvariant();

                var request = $"CONNECT {Config.Address}:{Config.Port} HTTP/1.1{endOfLine}" +
                    $"Connection: keep-alive{endOfLine}" +
                    $"Proxy-Connection: keep-alive{endOfLine}" +
                    $"Host: {Config.Address}:{Config.Port}{endOfLine}";

                if (Config.Proxy.PreAuthenticate)
                    switch (Config.Proxy.Authentication)
                    {
                        case ProxyAuthentication.Auto: break;

                        case ProxyAuthentication.Basic:

                        var credentials = $"{Config.Proxy.UserName?.ToLowerInvariant()}:{Config.Proxy.Password}";
                        var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                        proxyAuthorization = $"Proxy-Authorization: Basic {base64Credentials}";
                        break;

                        case ProxyAuthentication.Digest: throw new InvalidOperationException("Proxy Digest Access Authentication is not yet supported");
                    }

                if (proxyAuthorization != null)
                    request += $"{proxyAuthorization}{endOfLine}";

                request += endOfLine;

                await Utilities.Task.Run(() =>
                {
                    var buffer = new byte[8192];

                    //System.Net.HttpStatusCode.ProxyAuthenticationRequired

                    var length = Encoding.UTF8.GetBytes(request, 0, request.Length, buffer, 0);

                    var offset = 0;

                    while (offset < length)
                        offset += TcpSocket.Send(buffer, offset, length - offset, SocketFlags.None);

                    length = TcpSocket.Receive(buffer);

                    if (length == 0)
                        return result = new Result(ResultCode.SocketError, comment: SocketError.ConnectionReset.ToString());

                    offset = 0;
                    var response = default(string);

                    while (true)
                    {
                        response += Encoding.UTF8.GetString(buffer, offset, length);

                        if (!response.Contains($"{endOfLine}{endOfLine}"))
                            continue;


                        //System.Net.Http.HttpResponseMessage.
                        //var httpRequest = new HttpRequest(response);

                        foreach (var line in response.Split(endOfLine.ToCharArray()))
                        {
                            var normalizedLine = line.ToLowerInvariant();

                            if (normalizedLine.StartsWith(contentLengthHeader))
                            {
                                var indexOfContentLength = normalizedLine.IndexOf(contentLengthHeader);

                                if (!int.TryParse(line.Substring(indexOfContentLength), out contentLength))
                                    return result = new Result(ResultCode.Error);
                                else
                                    break;
                            }
                        }

                        if (contentLength != -1)
                        {
                            var headerLength = response.IndexOf($"{endOfLine}{endOfLine}");

                            //if (response.Length != headerLength + contentLength)
                            //    continue;
                        }

                        break;
                    }

                    return result;
                });

                return result;
            }
            catch (Exception exc)
            {
                return result = new Result(ResultCode.Exception, exception: exc);
            }
        }
        
        public virtual async Task<Result> AuthenticateAsync(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            var result = Result.Success;

            if (IsAuthenticated)
                return result;

            var srpClient = default(SrpClient);
            var startTime = DateTime.Now;
            var remainingTime = Timeout.Infinite;

            var GetRemainingTime = new Func<int>(() => millisecondsTimeout == Timeout.Infinite ? Timeout.Infinite :
                millisecondsTimeout - (int)(DateTime.Now - startTime).TotalMilliseconds);

            try
            {
                Connection.State = ConnectionState.Authenticating;

                //if (Config.Srp.Enabled)
                if (Config.AuthenticationMode == AuthenticationSchema.SrpProtocol)
                {
                    srpClient = new SrpClient(Config.Name, Config.Password, Config.Srp.InternalConfig);

                    var password = Config.AuthenticationMode == AuthenticationSchema.SrpProtocol ? srpClient.A : Config.InsecurePassword;

                    var authRequest = new AuthRequest { I = Config.Name, A = password, Schema = Config.AuthenticationMode };

                    var validationErrors = authRequest.Validate(this);

                    if (validationErrors != null)
                        return result = new Result(ResultCode.Error, model: validationErrors);

                    Connection.State = ConnectionState.Authenticating;

                    remainingTime = GetRemainingTime();

                    if (remainingTime < 0 && remainingTime != Timeout.Infinite)
                        return new Result(ResultCode.OperationTimedOut);
                    else if (cancellationToken.IsCancellationRequested)
                        return new Result(ResultCode.OperationCanceled);

                    using (var packet = new Packet(this) { Model = authRequest })
                    {
                        if (!(result = await packet.QueryAsync(remainingTime, cancellationToken).ConfigureAwait(false)))
                            return result;

                        var authResponse = packet.Response.GetModel<AuthResponse>();

                        if (!(result = authResponse.Result))
                            return result;

                        if (Config.AuthenticationMode == AuthenticationSchema.SrpProtocol)
                            Link.Crypto.Algorithm = srpClient.Calculate(authResponse.s, authResponse.B);
                    }
                }

                var loginRequest = new LoginRequest
                {
                    Srp_M = srpClient?.M,
                    Name = Config.Name,
                    Reset = Config.LoginReset,
                    NoDelay = Config.TcpNoDelay,
                    Services = Config.Services,
                    CustomData = Config.CustomData,
                    UdpEnabled = Config.UdpEnabled// ? (UdpSocket.LocalEndPoint as IPEndPoint).Port : 0
                };

                if (!(result = loginRequest.Validate(this)))
                    return result;

                var loginResponse = (LoginResponse)null;

                remainingTime = GetRemainingTime();

                if (remainingTime < 0 && remainingTime != Timeout.Infinite)
                    return new Result(ResultCode.OperationTimedOut);
                else if (cancellationToken.IsCancellationRequested)
                    return new Result(ResultCode.OperationCanceled);

                using (var packet = new Packet(this) { Model = loginRequest })
                {
                    //if (Config.Srp.Enabled)
                    if (Config.AuthenticationMode == AuthenticationSchema.SrpProtocol)
                        packet.Encrypt = true;

                    if (!(result = await packet.QueryAsync(remainingTime, cancellationToken).ConfigureAwait(false)))
                        return result;

                    loginResponse = packet.Response.GetModel<LoginResponse>();

                    Connection.FirstLogin = loginResponse.FirstLogin;
                    Connection.CustomData = loginResponse.CustomData;
                    NetworkInformation.IsUDPEnabled = loginResponse.UdpAllowed;

                    if (loginResponse.UdpAllowed)
                    {
                        if (!Config.UdpEnabled)
                            throw new InvalidOperationException("Protocol Error.");

                        var localEndPoint = TcpSocket.LocalEndPoint as IPEndPoint;
                        var udpIPEndPoint = new IPEndPoint(localEndPoint.Address, 0);

                        UdpSocket.Bind(udpIPEndPoint);
                        UdpSocket.Connect(RemoteEndPoint);

                        Link.Receives.SocketReceiveAsync(UdpSocket);

                        var udpHello = new UdpHello { Key = loginResponse.UdpKey };
                        using (var packetReply = new Packet(packet.Response.Reply) { Model = udpHello, Protocol = PacketProtocol.Udp })
                        {
                            var udpQueryTime = 100;

                            do
                            {
                                remainingTime = GetRemainingTime();

                                if (remainingTime < 0 && remainingTime != Timeout.Infinite)
                                    return new Result(ResultCode.OperationTimedOut);
                                else if (cancellationToken.IsCancellationRequested)
                                    return new Result(ResultCode.OperationCanceled);

                                if (remainingTime < udpQueryTime)
                                    udpQueryTime = remainingTime;

                                result = await packetReply.QueryAsync(remainingTime, cancellationToken).ConfigureAwait(false);

                                udpQueryTime += udpQueryTime / 2;
                            }
                            while (result == ResultCode.OperationTimedOut);
                        }
                    }

                    if (!(result = loginResponse.Result))
                        return result;
                }

                //if (Config.Srp.Enabled)
                if (Config.AuthenticationMode == AuthenticationSchema.SrpProtocol)
                    if (!srpClient.Verify(loginResponse.SrpM))
                        return result = new Result(ResultCode.Srp_M_Mismatch);

                srpClient?.Dispose();

                Config.SetName(loginResponse.Name);

                Connection.State = ConnectionState.Authenticated;

                //if (result && client.NetworkInformation.IsUDPEnabled)
                //    LocalEndPoint = UdpSocket.LocalEndPoint;
                //else if (client.NetworkInformation.IsUDPEnabled)
                //    Utilities.Socket.Close(UdpSocket);
                //else

                //{
                //    Utilities.Socket.Close(UdpSocket);
                //    LocalEndPoint = TcpSocket.LocalEndPoint;
                //    Log(LogCategory.Warning, "Can't setup UDP protocol because it is disabled at server side.");
                //}

                return result;
            }
            catch (Exception exc)
            {
                return result = new Result(ResultCode.Exception, exception: exc);
            }
            finally
            {

            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
