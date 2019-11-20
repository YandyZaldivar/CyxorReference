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
using System.Threading;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Cyxor.Networking
{
    using Extensions;
    using Serialization;

    using static Utilities.Threading;

    sealed class Delivery : Awaitable, IDisposable, IRecyclable
    {
        int Id;

        bool Recycled;
        bool Disposed;
        bool Initialized;

        Serializer CryptoHeader;
        Serializer CryptoPayload;

        Node Node;
        Connection Connection;
        Connection RelayConnection;
        SocketAsyncEventArgs SendAsyncSaea;
        InterlockedInt QueryInterlocked = new InterlockedInt();
        QueryType Type => Packet.Internal ? QueryType.Internal : QueryType.Standard;

        internal Packet Packet { get; private set; }
        internal Delivery ReplacedPartial { get; set; }
        internal Packet ReplyPacket { get; private set; }

        CancellationToken CancellationToken { get; set; }
        CancellationTokenSource TimeoutCancellation { get; set; }
        CancellationTokenRegistration CancelRegistration { get; set; }
        CancellationTokenRegistration TimeoutRegistration { get; set; }

        bool IsCanceledOrTimedOut => IsCanceled || IsTimedOut;
        bool IsCanceled => CancellationToken.IsCancellationRequested;
        bool IsTimedOut => TimeoutCancellation?.IsCancellationRequested ?? false;
        //bool IsTimedOut => TimeoutCancellation == null ? false : TimeoutCancellation.IsCancellationRequested;

        public Delivery(Node node)
        {
            Node = node;
            Recycled = true;
            Action = SendAsync;
            SendAsyncSaea = new SocketAsyncEventArgs();
            SendAsyncSaea.Completed += ProcessSend;
            CryptoPayload = Node.Pools.PopBuffer();
            CryptoHeader = new Serializer(new byte[32], 0, 32);
        }

        public void SetBuffer(byte[] buffer, int offset, int count) => SendAsyncSaea.SetBuffer(buffer, offset, count);


        //void Validate(bool skipNodeCheck = false)
        //{
        //    if (Disposed)
        //        throw new ObjectDisposedException(nameof(Delivery), Utilities.Strings.CyxorInternalException);

        //    if (Recycled)
        //        throw new InvalidOperationException($"{Utilities.Strings.CyxorInternalException}: The object can't be used while recycled.");

        //    if (!Initialized)
        //        throw new InvalidOperationException($"{Utilities.Strings.CyxorInternalException}: The object must be initialized before using.");
        //}

        public void Initialize(Connection connection, Packet packet, CancellationToken cancellationToken, int millisecondsTimeout)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(Delivery), Utilities.ResourceStrings.CyxorInternalException);

            if (!Recycled)
                throw new InvalidOperationException($"{Utilities.ResourceStrings.CyxorInternalException}: The object must be recycled before initializing.");

            if (Initialized)
                throw new InvalidOperationException($"{Utilities.ResourceStrings.CyxorInternalException}: The object have been already initialized.");

            if (connection == null)
                throw new ArgumentNullException(nameof(connection), Utilities.ResourceStrings.CyxorInternalException);

            if (packet == null)
                throw new ArgumentNullException(nameof(packet), Utilities.ResourceStrings.CyxorInternalException);

            if (Connection != null)
                throw new InvalidOperationException("The Link property have been already set.");

            //if (!Node.Config.Ssl.Enabled || connection.Link.Ssl.IsAuthenticated)
            if (!connection.TryAcquireSends())
            {
                TrySetResult(new Result(ResultCode.Error, "The packet can't be sent because the destination endpoint is no longer connected."));
                return;
            }

            if (packet.RelayConnection?.TryAcquireReceives(packet) ?? false)
                RelayConnection = packet.RelayConnection;

            Packet = packet;
            Recycled = false;
            Initialized = true;
            Connection = connection;

            if (Node is Server)
                if (packet.Protocol == PacketProtocol.Udp)
                    SendAsyncSaea.RemoteEndPoint = Connection.Link.RemoteUDPEndPoint;

            CancellationToken = cancellationToken;

            if (millisecondsTimeout != Timeout.Infinite)
            {
                TimeoutCancellation = new CancellationTokenSource();
                TimeoutCancellation.CancelAfter(millisecondsTimeout);
            }

            if (Packet.IsQuery && !Packet.IsRelay)
            {
                Id = Connection.Link.Queries.Add(this, Type);

                Packet.ReadOnly = false;
                Packet.QueryId = Id;
                Packet.ReadOnly = true;

                if (CancellationToken != CancellationToken.None)
                    CancelRegistration = CancellationToken.Register(CanceledCallback, useSynchronizationContext: false);

                if (TimeoutCancellation != null)
                    TimeoutRegistration = TimeoutCancellation.Token.Register(TimedOutCallback, useSynchronizationContext: false);
            }

            SetBuffer(packet.SerialBuffer.Buffer, 0, packet.SerialBuffer.Length);
        }

        bool TrySetQueryResult(Result result)
        {
            if (QueryInterlocked.Increment() == 2)
            {
                if (result == Result.Success)
                    if (IsCanceled)
                        result = new Result(ResultCode.OperationCanceled);
                    else if (IsTimedOut)
                        result = new Result(ResultCode.OperationTimedOut);

                TrySetResult(result);
                return true;
            }

            return false;
        }

        void TryCancelQuery(Result result, [CallerMemberName] string callerName = null)
        {
            if (Packet.IsQuery)
            {
                if (Connection.Link.Queries.TryRemove(Id, Type, out var delivery, cancelled: true))
                {
                    if (delivery != this)
                        Node.Log(LogCategory.Fatal, $"Wrong delivery obtained in '{callerName}'.");

                    if (!TrySetQueryResult(result))
                        if (!Connection.Link.Queries.TryRemoveCancelled(Id, Type))
                            Node.Log(LogCategory.Fatal, $"Failed to remove canceled query in '{callerName}'");
                }
            }
        }

        void CanceledCallback()
        {
            TimeoutRegistration.Dispose();
            TryCancelQuery(new Result(ResultCode.OperationCanceled));
        }

        void TimedOutCallback()
        {
            CancelRegistration.Dispose();
            TryCancelQuery(new Result(ResultCode.OperationTimedOut));
        }

        void ForbidCancellation()
        {
            CancellationToken = CancellationToken.None;
            TimeoutCancellation?.Dispose();
            TimeoutCancellation = null;

            CancelRegistration.Dispose();
            TimeoutRegistration.Dispose();
        }

        void SendAsync()
        {
            if (IsCompleted)
                return;

            var queuedReferencesCount = 0;

            if (Packet.Sending != PacketSending.Overlapped)
            {
                queuedReferencesCount = Connection.Link.Sends.AcquireQueue();

                if (queuedReferencesCount > 1)
                    Connection.Link.Sends.AddDelivery(this);
            }

            if (Packet.Box.Sending == PacketSending.Overlapped || queuedReferencesCount == 1)
                SocketSendAsync();
        }

        void SocketSendAsync()
        {
            if (!Packet.IsQuery && !IsCanceledOrTimedOut)
                ForbidCancellation();

            if (IsCanceledOrTimedOut)
                ProcessSend(sender: null, saea: SendAsyncSaea);
            else if (Connection.Link.GetPacketSocket(Packet) == Node.UdpSocket && Connection.UdpEnabled)
            {
                if (!Node.UdpSocket.SendToAsync(SendAsyncSaea))
                    ProcessSend(Node.UdpSocket, SendAsyncSaea);
            }
            else
            {
                //if (Node.Config.Srp.Enabled)
                if (Node.Config.AuthenticationMode == Config.AuthenticationSchema.SrpProtocol)
                {
                    // TODO: SRP Streaming Encryption (SSL Like)

                    if (Packet.Encrypt)
                    {
                        //var arraySegment = Connection.Link.Crypto.Encrypt(Packet.Frame.Payload);
                        //SendAsyncSaea.SetBuffer()

                        var bits = (BitSerializer)0;
                        bits[ProtocolMap.Encrypted] = true;

                        // Write bits into the protocol header
                    }
                }

                if (Node.Config.Ssl.Enabled)
                {
                    var serializer = Packet.Serializer;

                    if (!Connection.Link.Ssl.IsAuthenticated)
                        SendAsyncSaea.SetBuffer(serializer.Buffer, 0, serializer.Length);
                    else
                    {
                        //if (Packet.Id == (int)InternalCoreApiId.HttpPage)
                        //    SendAsyncSaea.SetBuffer(serializer.Buffer, 0, serializer.Length);

                        Connection.Link.Ssl.Write(SendAsyncSaea);
                    }
                }

                if (!Connection.Link.TcpSocket.SendAsync(SendAsyncSaea))
                    ProcessSend(Connection.Link.TcpSocket, SendAsyncSaea);
            }
        }

        void ProcessSend(object sender, SocketAsyncEventArgs saea)
        {
            if (sender != null)
            {
                Node.Statistics.LastOperationDate = DateTime.Now;
                Connection.Statistics.LastOperationDate = DateTime.Now;

                if (saea.SocketError == SocketError.Success)
                {
                    Node.Statistics.AddSentBytes(saea.BytesTransferred);
                    Connection.Statistics.AddSentBytes(saea.BytesTransferred);
                }

                if (saea.SocketError == SocketError.Success && saea.BytesTransferred < saea.Count)
                {
                    saea.SetBuffer(saea.BytesTransferred, saea.Count - saea.BytesTransferred);
                    SocketSendAsync();

                    // TODO: Remove! Just to know if dotnet infrastructure informs back send progress.
                    Node.Log(LogCategory.Information, "Packet delivery trunked: '{0}'.", Packet.ToString());

                    return;
                }

                if (saea.SocketError == SocketError.Success)
                {
                    //if (Packet.Internal && Packet.Id == (int)InternalCoreApiId.Shutdown && Packet.Protocol == PacketProtocol.Tcp)
                    if (Packet.Id == Utilities.HashCode.GetFrom("shutdown") && Packet.Protocol == PacketProtocol.Tcp)
                        if (Connection.DisconnectionSource == DisconnectionSource.Local)
                            Connection.Link.TcpSocket.Shutdown(SocketShutdown.Send);
                        else if (Connection.DisconnectionSource == DisconnectionSource.Remote)
                            Connection.Link.TcpSocket.Shutdown(SocketShutdown.Both);
                        else Node.Log(LogCategory.Fatal, "Invalid shutdown disconnection source.");
                }
            }

            if (Packet.Sending == PacketSending.Queued)
                if (Connection.Link.Sends.ReleaseQueue() > 0)
                    Connection.Link.Sends.TakeDelivery().SocketSendAsync();

            Connection.ReleaseSends();
            RelayConnection?.ReleaseReceives(Packet);

            var errorResult = new Result(ResultCode.SocketError, saea.SocketError.ToString());

            if (Packet.IsQuery)
            {
                if (saea.SocketError == SocketError.Success)
                    TrySetQueryResult(Result.Success);
                else
                {
                    ForbidCancellation();

                    if (!TrySetQueryResult(errorResult))
                    {
                        if (!Connection.Link.Queries.TryRemove(Id, Type, out var delivery, cancelled: false))
                            Node.Log(LogCategory.Fatal, "Failed to remove delivery in process send error.");
                        else
                        {
                            if (delivery != this)
                                Node.Log(LogCategory.Fatal, "Wrong delivery obtained in process send error.");

                            if (!TrySetQueryResult(errorResult))
                                Node.Log(LogCategory.Fatal, "Failed to set delivery error result in process send.");
                        }
                    }
                }
            }
            else
            {
                if (saea.SocketError != SocketError.Success)
                    TrySetResult(errorResult);
                else if (IsCanceled)
                    TrySetResult(new Result(ResultCode.OperationCanceled));
                else if (IsTimedOut)
                    TrySetResult(new Result(ResultCode.OperationTimedOut));
                else
                    TrySetResult(Result.Success);

                ReplacedPartial?.ForbidCancellation();
                ReplacedPartial?.ProcessSend(null, saea);
            }
        }

        internal static bool TryProcessReply(Connection connection, Packet packet)
        {
            if (!packet.IsReply || packet.IsRelay)
                return false;

            if (connection.Node is Server)
                if (!packet.IsToNode)
                    return false;

            var queryType = packet.Internal ? QueryType.Internal : QueryType.Standard;

            if (connection.Link.Queries.TryRemove(packet.ReplyId, queryType, out var delivery, cancelled: false))
            {
                delivery.ForbidCancellation();
                delivery.ReplyPacket = packet;
                delivery.TrySetQueryResult(Result.Success);
            }
            else
            {
                if (connection.Link.Queries.TryRemoveCancelled(packet.ReplyId, queryType))
                {
                    // Delayed reply
                    if (!packet.Internal)
                    {
                        // TODO: This is a non-internal delayed query. Decide if raise an event with the information.
                        packet.Dispose();
                    }
                }
                else
                {
                    // TODO: Throw Protocol Error?

                    // If a query reply comes from a remote client to another client and the destination connection time
                    // is after the time the query was issued, drop the query. Then if we receive a misplaced  query reply
                    // we can treat is as a protocol error.
                }
            }

            return true;
        }

        void IRecyclable.Reset() => Reset();

        new void Reset() => base.Reset();

        void IRecyclable.Recycle() => Recycle();

        internal void Recycle()
        {
            if (!Recycled)
            {
                if (!IsCompleted)
                    Node.Log(LogCategory.Fatal, $"Resetting {nameof(Delivery)} class instance while still running.");

                if (Packet.IsQuery)
                    if (QueryInterlocked.Exchange(0) != 2)
                        Node.Log(LogCategory.Fatal, $"'{nameof(QueryInterlocked)}' field reset failed in the '{nameof(Delivery)}' class.");

                SendAsyncSaea.RemoteEndPoint = null;
                SendAsyncSaea.SetBuffer(null, 0, 0);

                ForbidCancellation();

                Id = 0;
                Packet = null;
                Recycled = true;
                Connection = null;
                ReplyPacket = null;
                Initialized = false;
                RelayConnection = null;
                ReplacedPartial = null;

                Reset();

                Node.Pools.PushDelivery(this);
            }
        }

        void IDisposable.Dispose()
        {
            if (!Disposed)
            {
                Reset();

                SendAsyncSaea.Completed -= ProcessSend;
                SendAsyncSaea.Dispose();
                Disposed = true;
                Node = null;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
