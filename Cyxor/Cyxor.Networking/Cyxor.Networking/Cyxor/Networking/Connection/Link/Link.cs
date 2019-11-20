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
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Cyxor.Networking
{
    using Models;
    using Controllers;

#if NET35
    using Extensions;
#endif

    using static Utilities.Threading;

    sealed partial class Link : Node.NodeProperty, IAwaiter, IDisposable
    {
        bool Disposed;

        Awaitable awaitable;
        Awaitable Awaitable
        {
            get
            {
                Interlocked.CompareExchange(ref awaitable, ExchangeAwaitable, null);
                return awaitable;
            }
        }

        Awaitable exchangeAwaitable = null;
        Awaitable ExchangeAwaitable => exchangeAwaitable ?? (exchangeAwaitable = new Awaitable());

        public Awaitable GetAwaiter() => Awaitable;
        public Awaitable ConfigureAwait(bool continueOnCapturedContext) => Awaitable.ConfigureAwait(continueOnCapturedContext);


        //Awaitable awaitable;
        //internal Awaitable GetAwaiter() => Awaitable;
        //Awaitable IAwaiter.GetAwaiter() => GetAwaiter();
        //bool IAwaiter.IsCompleted => Awaitable.IsCompleted;
        //Result IAwaiter.GetResult() => Awaitable.GetResult();
        //Awaitable Awaitable => awaitable ?? (awaitable = new Awaitable());
        //internal Awaitable ConfigureAwait(bool continueOnCapturedContext) => Awaitable;
        //Awaitable IAwaiter.ConfigureAwait(bool continueOnCapturedContext) => ConfigureAwait(continueOnCapturedContext);
        //void INotifyCompletion.OnCompleted(Action continuation) => (Awaitable as INotifyCompletion).OnCompleted(continuation);

        InterlockedInt DisconnectReferences;
        InterlockedInt InterlockedDisconnect;

        internal Socket TcpSocket;

        //internal SslStream SslStream;
        //internal LinkStream SslBackendStream;

        internal LinkSsl Ssl { get; set; }
        internal LinkSends Sends { get; set; }
        internal LinkCrypto Crypto { get; set; }
        internal LinkQueries Queries { get; set; }
        internal LinkReceives Receives { get; set; }
        internal Connection Connection { get; set; }

        internal ConcurrentDictionary<Type, Controller> ControllerInstances { get; }

        public Link(Node node) : base(node)
        {
            Ssl = new LinkSsl(this);
            Sends = new LinkSends(this);
            Crypto = new LinkCrypto(this);
            Queries = new LinkQueries(this);
            Receives = new LinkReceives(this);

            Connection = Node.CreateConnection();
            Connection.Link = this;

            DisconnectSaea = new SocketAsyncEventArgs { UserToken = this };

            ControllerInstances = new ConcurrentDictionary<Type, Controller>();
        }


        public LinkNoob Noob = new LinkNoob();

        internal CancellationTokenSource LoginTimeoutCts = new CancellationTokenSource();


        internal int AddressHash { get; private set; }
        internal IPEndPoint RemoteUDPEndPoint;





        //internal Result ShutdownResult;

        //internal string DisconnectSocketError;
        //internal string RemoteDisconnectReason;
        //internal TaskCompletionSource<Result> DisconnectedTcs;
        internal SocketAsyncEventArgs DisconnectSaea { get; set; }
        internal InterlockedInt DisconnectSocketReference;

        internal int DisconnectReferencesIncrement()
        {
            var recycle = DisconnectReferences.Increment();

            if (recycle == 2)
                Shutdown();

            return recycle;
        }

        void IDisposable.Dispose() => Dispose();

        public override int GetHashCode() => Connection.GetHashCode();

        internal Socket GetPacketSocket(Packet packet) => packet.Protocol == PacketProtocol.Tcp ? TcpSocket : Node.UdpSocket;

        internal async Task<Result> ConnectAsync(Socket socket)
        {
            //if (Connection.State != ConnectionState.Recycled)
            //    Node.Log(LogCategory.Fatal, $"{nameof(Link)} connection attempt with invalid state '{Connection.State}'.");

            var result = Result.Success;
            //var server = Node as Server;

            TcpSocket = socket;

            //if (Sends.Acquire() != 1)
            //    Node.Log(LogCategory.Fatal, "Send references increment invalid initial state.");

            ////if (Receives.Acquire(socket: null) != 1)
            ////    Node.Log(LogCategory.Fatal, "Receive references increment invalid initial state.");

            Connection.RemoteEndPoint = TcpSocket.RemoteEndPoint as IPEndPoint;
            Connection.Statistics.ConnectionDate = DateTime.Now;
            Node.Statistics.ConnectionDate = Connection.Statistics.ConnectionDate;

            AddressHash = (TcpSocket.LocalEndPoint.ToString() + TcpSocket.RemoteEndPoint.ToString()).GetHashCode();

            if (Node.IsServer)
                while (!Node.AsServer.Connections.TryAddToConnectedLinks(this))
                    AddressHash++;

            Node.AsServer?.Statistics?.ConnectionsIncrement();

            if (result)
            {
                Sends.Initialize();
                Receives.Initialize();
            }

            if (Node.Config.Ssl.Enabled)
            {
                Connection.State = ConnectionState.SslHandshaking;
                result = await Ssl.ConnectAsync();
                Ssl.BytesBuffered = 0;
            }

            if (result)
            {
                Receives.SocketReceiveAsync(TcpSocket);
                Connection.State = ConnectionState.Connected;

                Node.Log(LogCategory.ClientIn, $"[{DateTime.Now.ToString("HH:mm:ss")}] <{Connection.RemoteEndPoint.ToString()}> connected");
            }
            else
            {
                await DisconnectAsync(result, ShutdownSequence.Abortive);
            }

            return result;
        }

        internal Link DisconnectAsync(Result result, ShutdownSequence shutdownSequence = ShutdownSequence.Graceful, DisconnectionSource disconnectionSource = DisconnectionSource.None)
        {
            if (InterlockedDisconnect.CompareExchange(1, 0) != 0)
                return this;

            new Action(async () =>
            {
                try
                {
                    var server = Node as Server;

                    if (disconnectionSource == DisconnectionSource.None)
                        disconnectionSource = DisconnectionSource.Local;

                    Connection.DisconnectionSource = disconnectionSource;

                    if (Connection.DisconnectionSource == DisconnectionSource.Local)
                        Connection.Result = result;                    

                    if (!(TcpSocket?.Connected ?? false))
                    {

                    }
                    else if (TcpSocket?.Connected ?? false)
                    {
                        //if (Connection.State != ConnectionState.Connected)
                        //    Node.Log(LogCategory.Fatal, "Connection and socket state mismatch.");

                        if (shutdownSequence == ShutdownSequence.Abortive)
                        {
                            TcpSocket.Shutdown(SocketShutdown.Both);
                        }
                        else if (shutdownSequence == ShutdownSequence.Immediate)
                        {
                            TcpSocket.Shutdown(SocketShutdown.Both);
                            TcpSocket.Dispose();
                        }
                        // NOTE: We prefer a hard shutdown for unknown connections
                        else if (Connection.IsHttp ?? true)
                        {
                            TcpSocket.Shutdown(SocketShutdown.Both);
                            TcpSocket.Dispose();
                        }
                        else if (Connection.State == ConnectionState.Connected || Connection.State == ConnectionState.Authenticated)
                        {
                            if (server != null)
                                await Node.Events.Post(new Events.Server.ClientDisconnectingEventArgs(Connection)).ConfigureAwait(false);
                            else
                                await Node.Events.Post(new Events.DisconnectingEventArgs(Node, Connection.Result.Comment)).ConfigureAwait(false);
                        }

                        if (TcpSocket.Connected)
                        {
                            if (Connection.DisconnectionSource == DisconnectionSource.Remote)
                                TcpSocket.Shutdown(SocketShutdown.Both);
                            else if (Connection.DisconnectionSource == DisconnectionSource.Local)
                                using (var packet = new Packet(Connection) { Model = new ShutdownApiModel { Reason = Connection.Result.Comment } })
                                    await packet.SendAsync().ConfigureAwait(false);
                        }
                    }
                }
                catch
                {
                    // The socket may error on shutdown
                }
                finally
                {
                    Connection.State = ConnectionState.Disconnecting;
                    Connection.ReleaseSends();
                }
            }).Invoke();

            return this;
        }

        void Shutdown()
        {
            try
            {
                if (TcpSocket.Connected)
                    Node.Log(LogCategory.Fatal, "Socket still connected after link shutdown.");

                if (Connection.State != ConnectionState.Disconnecting)
                    Node.Log(LogCategory.Fatal, "Invalid connection state on link shutdown.");

                Ssl.Reset();

                TcpSocket.Dispose();
                TcpSocket = null;

                Node.UdpSocket?.Dispose();
                Node.UdpSocket = null;

                Connection.State = ConnectionState.Disconnected;

                // TODO: Discard pending queries, etc.
                //Queries.Reset();

                if (Node.AsClient != null)
                {
                    Node.Events.Post(new Events.DisconnectCompletedEventArgs(Node, Connection.Result.Comment));
                }
                else
                {
                    var server = Node as Server;

                    //if (link.Connection.State != ConnectionState.Registering)
                    server.Connections.TryRemoveFromConnectedLinks(this);

                    if (server.Connections.Exists(this))
                    {
                        //server.Protocol.SpreadClientService(Connection, ClientServices.ClientDisconnected);
                        server.Connections.TryRemoveFromAuthenticatedLinks(this);
                        server.Statistics.AuthenticatedConnectionsDecrement();

                    }

                    Node.Log(LogCategory.ClientOut, $"[{DateTime.Now.ToString("HH:mm:ss")}] <{Connection.RemoteEndPoint.ToString()}> disconnected");

                    server.Statistics.ConnectionsDecrement();

                    Node.Events.Post(new Events.Server.ClientDisconnectedEventArgs(this));
                }
            }
            finally
            {


                if (Interlocked.CompareExchange(ref awaitable, Awaitable.CompletedAwaitable, null) != null)
                    awaitable.TrySetResult(Result.Success);
            }
        }

        internal void Reset()
        {
            exchangeAwaitable?.Reset();
            awaitable = exchangeAwaitable;

            //if (boxQueue.Count != 0)
            //   throw new CyxorNetworkException("Link box queue is not empty");

            //if (SendReferences.Value != 0 || RecvReferences.Value != 0)
            //   throw new CyxorNetworkException("Link references are not empty");

            Connection?.Dispose();
            Connection = Node.CreateConnection();
            Connection.Link = this;

            Sends.Reset();
            Receives.Reset();

            LoginTimeoutCts = new CancellationTokenSource();

            Noob.Reset();
            Crypto?.Dispose();

            //DisconnectedTcs = null;
            //DisconnectSocketError = null;
            //RemoteDisconnectReason = null;
            InterlockedDisconnect.Exchange(0);
            DisconnectSocketReference.Exchange(0);

            //Channels.Clear();

            //PacketSize = 0;
            //CheckBuffersAndBoxs();



            var referenceCountCheck = 0;

            //referenceCountCheck |= References.Value;
            //referenceCountCheck |= TcpReferences.Value;
            //referenceCountCheck |= UdpReferences.Value;

            if (referenceCountCheck != 0)
                Node.Log(LogCategory.Fatal, "Link Receive reference count incorrect value state.");
        }

        internal void Recycle()
        {
            var name = Connection.Name;
            var address = Connection.RemoteEndPoint;
            var previousClientState = Connection.State;

            if (Node is Server)
            {
                Connection.State = ConnectionState.Disconnected;

                if (previousClientState >= ConnectionState.Connected)
                    Node.Events.Post(new Events.Server.ClientDisconnectedEventArgs(this));

                //if (previousClientState != ConnectionState.Accepting && previousClientState != ConnectionState.Registering)
                //    Awaitable.TrySetResult(Result.Success);

                if (previousClientState != ConnectionState.Connecting)
                    if (DisconnectReferences.Exchange(0) != 2)
                        Node.Log(LogCategory.Fatal, "The link is not in a valid state for reseting.");
            }

            Reset();
            Node.Pools.Push(this);

            if (previousClientState != ConnectionState.Connecting)
            {
                // Node.Log(LogCategory.Log, "<{0}> Recycling connection resources.", name ?? address.ToString());
            }
        }

        internal void Dispose()
        {
            if (!Disposed)
            {
                TcpSocket?.Dispose();

                Noob.Dispose();
                (Receives as IDisposable).Dispose();

                Disposed = true;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
