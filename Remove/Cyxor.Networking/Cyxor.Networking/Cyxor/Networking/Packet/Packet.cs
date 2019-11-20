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
using System.IO;
using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using System.Reflection;

namespace Cyxor.Networking
{
    using Models;
    using Extensions;
    using Serialization;

    using static Utilities.Threading;

    // A Packet internal cannot be partial and a query cannot be partial. Put on validations.
    public sealed class Packet : IBox, IDisposable
    {
        bool Disposed;
        //bool DisposeRequested;
        InterlockedInt Locker;

        public PacketResponse Response { get; internal set; }
        public IEnumerable<PacketResponse> ResponseList { get; internal set; }

        internal IReferenceCounted ReferenceCounted { get; set; }
        public Reference Reference => new Reference(ReferenceCounted);

        //internal Packet CreateReplyInternal
        //    (int id = 0,
        //    object message = null,
        //    PacketProtocol protocol = PacketProtocol.Tcp,
        //    Connection relayConnection = null) =>
        //    new Packet(Connection)
        //    {
        //        Id = id,
        //        ReplyId = QueryId,
        //        QueryMode = PacketQueryMode.Reply,
        //        Internal = Internal,
        //        Protocol = protocol,
        //        RelayConnection = relayConnection,

        //        Message = message
        //    };

        //public Packet CreateReply(int id = 0, object message = null, PacketProtocol protocol = PacketProtocol.Tcp) =>
        //    CreateReplyInternal(id, message, protocol, relayConnection: null);

        // TODO: Critical, fix!!!
        Box iBox;
        internal Box Box
        {
            get
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Packet));

                return iBox;
            }
            set
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Packet));

                iBox = value;
            }
        }

        // TODO: Critical, fix!!!
        //Box iBox;
        //internal Box Box
        //{
        //    get
        //    {
        //        //if (Disposed)
        //        //    throw new ObjectDisposedException(nameof(Packet));

        //        return iBox;
        //    }
        //    set
        //    {
        //        //if (Disposed)
        //        //    throw new ObjectDisposedException(nameof(Packet));

        //        iBox = value;
        //    }
        //}

        public void Dispose()
        {
            // TODO: Lock with readonly to only allow users dispose their own packets.
            //       Just before internal dispose set readonly to false. Analise this.

            if (!Disposed)
            {
                if (Locker.CompareExchange(1, 0) != 0)
                {
                    //DisposeRequested = true;
                    return;
                }

                if (iBox != null)
                {
                    if (iBox.ReadOnly)
                    {
                        //int recvReferencesCount = iBox.Link.RecvReferences.Decrement();

                        //if (recvReferencesCount == 1)
                        //   SocketReceiveAsync(socket, e);
                        //else if (recvReferencesCount == 0)
                        //   if (iBox.Link.RecycleReferences.Increment() == 2)
                        //      Factory.PushLink(iBox.Link);
                    }

                    if (iBox != null)
                        Node.Pools.PushBox(iBox);

                    Response.Dispose();

                    if (ResponseList != null)
                        foreach (var item in ResponseList)
                            item.Dispose();

                    iBox = null;
                }

                Disposed = true;
                Locker.Exchange(0);
            }
        }

        public void Reset() => Box.Reset();

        internal Result Validate()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(Packet), Utilities.ResourceStrings.CyxorInternalException);

            return Box.Validate();
        }

        //internal Result Result => Serializer.DeserializeRawObject<Result>();

        internal Result Result => Serializer.ToObject<Result>();

        public T GetModel<T>() => Result.GetModel<T>();
        public T GetModel<T>(T value) => Result.GetModel(value);
        public object GetModel(Type type) => Result.GetModel(type);
        public void PopulateObject<T>(T value) => Result.PopulateObject(value);
        public T GetModel<T>(IBackingSerializer serializer) => Result.GetModel<T>(serializer);
        public T GetModel<T>(T value, IBackingSerializer serializer) => Result.GetModel(value, serializer);

        public object Model
        {
            set
            {
                var serializer = default(PacketSerializer);

                if (value != null)
                {
                    var modelAttr = value.GetType().GetTypeInfo().GetCustomAttribute<ModelAttribute>();

                    if (modelAttr != null)
                    {
                        Id = modelAttr.Id;
                        serializer = modelAttr.Serializer;
                    }
                }

                Serializer.Reset();

                var backingSerializer = default(IBackingSerializer);

                switch (serializer)
                {
                    case PacketSerializer.Cyxor: backingSerializer = null; break;
                    case PacketSerializer.Json: backingSerializer = new JsonBackingSerializer(); break;
                }

                if (!(value is Result) && !IsHttpMessage)
                    value = new Result(model: value, backingSerializer: backingSerializer);

                Serializer.SerializeRaw(value, backingSerializer);
            }
        }

#region IBoX

        public Node Node => Box.Node;

        public IEnumerable<Connection> Connections
        {
            get => Box.Connections;
            internal set => Box.Connections = value;
        }

        public Connection Connection
        {
            get => Box.Connection;
            internal set => Box.Connection = value;
        }

        public Connection RelayConnection
        {
            get => Box.RelayConnection;
            internal set => Box.RelayConnection = value;
        }

        public Connection RootConnection
        {
            get => Box.RootConnection;
            internal set => Box.RootConnection = value;
        }

        public bool IsHttpMessage
        {
            get => Box.IsHttpMessage;
            internal set => Box.IsHttpMessage = value;
        }

        internal ControllerAction ControllerAction
        { 
            get => Box.ControllerAction;
            set => Box.ControllerAction = value;
        }

        public string Sender => Box.Sender;
        public Serializer Serializer => Box.Serializer;
        public bool IsReadOnly => Box.ReadOnly;
        public bool IsToNode => Box.IsToNode;
        public bool IsFromNode => Box.IsFromNode;
        public bool IsGroupAddress => Box.IsGroupAddress;

        public string TransmitFilesSearchPattern
        {
            get => Box.TransmitFilesSearchPattern;
            set => Box.TransmitFilesSearchPattern = value;
        }

        public SearchOption TransmitFilesSearchOption
        {
            get => Box.TransmitFilesSearchOption;
            set => Box.TransmitFilesSearchOption = value;
        }

        public PacketTransmitFilesThreadOptions TransmitFilesThreadOptions
        {
            get => Box.PacketTransmitFilesThreadOptions;
            set => Box.PacketTransmitFilesThreadOptions = value;
        }

        public bool Internal
        {
            get => Box.Internal;
            set => Box.Internal = value;
        }

        public bool TrackSendProgress
        {
            get => Box.TrackSendProgress;
            set => Box.TrackSendProgress = value;
        }

        public bool Anonymous
        {
            get => Box.Anonymous;
            set => Box.Anonymous = value;
        }

        public bool Broadcast
        {
            get => Box.Broadcast;
            set => Box.Broadcast = value;
        }

        public bool Compress
        {
            get => Box.Compress;
            set => Box.Compress = value;
        }

        public bool Encrypt
        {
            get => Box.Encrypt;
            set => Box.Encrypt = value;
        }

        public bool ReadOnly
        {
            get => Box.ReadOnly;
            internal set => Box.ReadOnly = value;
        }

        /// <summary>
        /// The overlapped mode doesn't have effect if SSL is enabled or SRP with symmetric encryption is enabled.
        /// </summary>
        /// <value>
        /// The sending.
        /// </value>
        public PacketSending Sending
        {
            get => Box.Sending;
            set => Box.Sending = value;
        }

        public PacketPriority Priority
        {
            get => Box.Priority;
            set => Box.Priority = value;
        }

        public PacketProtocol Protocol
        {
            get => Box.Protocol;
            set => Box.Protocol = value;
        }

        public PacketQueryMode QueryMode
        {
            get => Box.QueryMode;
            internal set => Box.QueryMode = value;
        }

        public bool IsRelay => Box.IsRelay;
        public bool IsQuery => Box.IsQuery;
        public bool IsReply => Box.IsReply;
        public bool IsQueryAndReply => Box.IsQueryAndReply;

        internal int QueryId
        {
            get => Box.QueryId;
            set => Box.QueryId = value;
        }

        internal int ReplyId
        {
            get => Box.ReplyId;
            set => Box.ReplyId = value;
        }

        public string Address
        {
            get => Box.Address;
            set => Box.Address = value;
        }

        public int Id
        {
            get => Box.Id;
            set => Box.Id = value;
        }

        internal bool IsCommand
        {
            get => Box.IsCommand;
            set => Box.IsCommand = value;
        }

        string route;
        public string Route
        {
            get => route;
            set
            {
                route = value;
                Id = Utilities.HashCode.GetFrom(route);
            }
        }

        string[] fileNames;
        public string[] FileNames
        {
            get
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Packet));

                return fileNames;
            }
            set
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Packet));

                if (ReadOnly)
                    throw new InvalidOperationException("This packet instance is readonly.");

                fileNames = value;
            }
        }

        #endregion

        #region Constructors

        public Packet(Node node, object model) : this(node) => Model = model;
        public Packet(Node node, int id, object model) : this(node, model) => Id = id;
        public Packet(Node node, string route, object model) : this(node, model) => Route = route;

        public Packet(Node node) : this(node?.Pools.PopBox())
            => Connection = node.AsClient?.Connection;

        public Packet(Connection connection, object model) : this(connection) => Model = model;
        public Packet(Connection connection, int id, object model) : this(connection, model) => Id = id;
        public Packet(Connection connection, string route, object model) : this(connection, model) => Route = route;

        public Packet(Connection connection) : this(connection?.Node)
            => Connection = connection;

        public Packet(IEnumerable<Connection> connections, object model) : this(connections) => Model = model;
        public Packet(IEnumerable<Connection> connections, int id, object model) : this(connections, model) => Id = id;
        public Packet(IEnumerable<Connection> connections, string route, object model) : this(connections, model) => Route = route;

        public Packet(IEnumerable<Connection> connections) : this(connections?.FirstOrDefault()?.Node)
        {
            var connectionList = new List<Connection>();

            foreach (var connection in connections)
                if (Node != connection?.Node)
                    throw new InvalidOperationException();
                else
                    connectionList.Add(connection);

            Connections = connectionList;
        }

        public Packet(Packet queryPacket, object model) : this(queryPacket) => Model = model;
        public Packet(Packet queryPacket, int id, object model) : this(queryPacket) => Id = id;
        public Packet(Packet queryPacket, string route, object model) : this(queryPacket) => Route = route;

        public Packet(Packet queryPacket) : this(queryPacket.Node)
        {
            if (!queryPacket.IsQuery)
                throw new ArgumentException("The packet you are trying to reply to is not a query.");

            IsHttpMessage = queryPacket.IsHttpMessage;

            Id = queryPacket.Id;
            ReplyId = queryPacket.QueryId;
            Internal = queryPacket.Internal;
            Protocol = queryPacket.Protocol;
            QueryMode = PacketQueryMode.Reply;
            Connection = queryPacket.RootConnection;

            if (Node.IsClient)
                Address = queryPacket.Sender;
        }

        internal Packet(Box box) => iBox = box ??
            throw new ArgumentException("Error initializing the packet, verify the provided arguments are valid.");


        #endregion Constructors

        //public Serializer SerialBuffer
        //{
        //    get => Box.SerialBuffer;
        //    //set { Box.SerialBuffer = value; }
        //}

        public Serializer SerialBuffer => Box.SerialBuffer;

        public void SaveToFile(string fileName) => SerialBuffer.SaveToFile(fileName);
        public void LoadFromFile(string fileName) => SerialBuffer.LoadFromFile(fileName);

        public Task<Result> SendAsync() => SendAsync(this, Timeout.Infinite, CancellationToken.None);
        public Task<Result> SendAsync(int millisecondsTimeout) => SendAsync(this, millisecondsTimeout, CancellationToken.None);
        public Task<Result> SendAsync(CancellationToken cancellationToken) => SendAsync(this, Timeout.Infinite, cancellationToken);
        public Task<Result> SendAsync(TimeSpan timeout) => SendAsync(this, (int)timeout.TotalMilliseconds, CancellationToken.None);
        public Task<Result> SendAsync(int millisecondsTimeout, CancellationToken cancellationToken) => SendAsync(this, millisecondsTimeout, cancellationToken);
        public Task<Result> SendAsync(TimeSpan timeout, CancellationToken cancellationToken) => SendAsync(this, (int)timeout.TotalMilliseconds, cancellationToken);

        public Task<Result> QueryAsync() => SendAsync(this, Timeout.Infinite, CancellationToken.None, true);
        public Task<Result> QueryAsync(int millisecondsTimeout) => SendAsync(this, millisecondsTimeout, CancellationToken.None, true);
        public Task<Result> QueryAsync(CancellationToken cancellationToken) => SendAsync(this, Timeout.Infinite, cancellationToken, true);
        public Task<Result> QueryAsync(TimeSpan timeout) => SendAsync(this, (int)timeout.TotalMilliseconds, CancellationToken.None, true);
        public Task<Result> QueryAsync(int millisecondsTimeout, CancellationToken cancellationToken) => SendAsync(this, millisecondsTimeout, cancellationToken, true);
        public Task<Result> QueryAsync(TimeSpan timeout, CancellationToken cancellationToken) => SendAsync(this, (int)timeout.TotalMilliseconds, cancellationToken, true);

        public Task<Result> CommandAsync() => SendAsync(this, Timeout.Infinite, CancellationToken.None, true, true);
        public Task<Result> CommandAsync(int millisecondsTimeout) => SendAsync(this, millisecondsTimeout, CancellationToken.None, true, true);
        public Task<Result> CommandAsync(CancellationToken cancellationToken) => SendAsync(this, Timeout.Infinite, cancellationToken, true, true);
        public Task<Result> CommandAsync(TimeSpan timeout) => SendAsync(this, (int)timeout.TotalMilliseconds, CancellationToken.None, true, true);
        public Task<Result> CommandAsync(int millisecondsTimeout, CancellationToken cancellationToken) => SendAsync(this, millisecondsTimeout, cancellationToken, true, true);
        public Task<Result> CommandAsync(TimeSpan timeout, CancellationToken cancellationToken) => SendAsync(this, (int)timeout.TotalMilliseconds, cancellationToken, true, true);

        //internal FramePacket Frame;

        //internal async Task<Result> RelayAsync(bool isQuery)
        //{

        //}

        static async Task<Result> SendAsync(Packet packet, int millisecondsTimeout, CancellationToken cancellationToken, bool isQuery = false, bool isCommand = false)
        {
            if (packet.Locker.CompareExchange(1, 0) != 0)
                return new Result(ResultCode.PacketSendingInProgress);

            var result = Result.Success;

            var sendsAcquired = false;
            var connectionSendsAcquired = default(List<Connection>);

            try
            {
                packet.IsCommand = isCommand;

                if (!packet.IsRelay)
                {
                    if (isQuery && packet.QueryMode == PacketQueryMode.Reply)
                        packet.QueryMode = PacketQueryMode.QueryAndReply;
                    else if (isQuery)
                        packet.QueryMode = PacketQueryMode.Query;
                    else if (packet.QueryMode != PacketQueryMode.Reply)
                        packet.QueryMode = PacketQueryMode.Send;
                }

                if (!(result = packet.Validate()))
                    return result;

                packet.ReadOnly = true;

                if (packet.Connection != null)
                {
                    sendsAcquired = packet.Connection.TryAcquireSends();

                    var delivery = packet.Node.Pools.PopDelivery();
                    delivery.Initialize(packet.Connection, packet, cancellationToken, millisecondsTimeout);
                    result = await delivery.ConfigureAwait(false);
                    packet.Response = new PacketResponse(delivery.ReplyPacket, result);
                    result = packet.Response.Result;
                    delivery.Recycle();
                }
                else if (packet.Connections != null)
                {
                    var parallelAwaitable = new Awaitable();
                    connectionSendsAcquired = new List<Connection>();
                    var connectionsCount = packet.Connections.Count();
                    var responseList = new ConcurrentStack<PacketResponse>();

                    Parallel.ForEach(packet.Connections, async (connection, pls, index) =>
                    {
                        try
                        {
                            if (connection.TryAcquireSends())
                                connectionSendsAcquired.Add(connection);

                            var delivery = packet.Node.Pools.PopDelivery();
                            delivery.Initialize(connection, packet, cancellationToken, millisecondsTimeout);
                            result = await delivery.ConfigureAwait(false);
                            responseList.Push(new PacketResponse(delivery.ReplyPacket, result));
                            delivery.Recycle();
                        }
                        finally
                        {
                            if (index == connectionsCount - 1)
                                parallelAwaitable.TrySetResult(Result.Success);
                        }
                    });

                    await parallelAwaitable.ConfigureAwait(false);

                    packet.ResponseList = responseList;

                    foreach (var packetResponse in packet.ResponseList)
                        if (!packetResponse.Result)
                        {
                            result = new Result(ResultCode.PacketSendMultiConnectionsErrors);
                            packet.Response = new PacketResponse(null, result);
                            break;
                        }
                }
                else
                {
                    result = new Result(ResultCode.ClientNotConnected);
                }

                return result;
            }
            catch (Exception ex)
            {
                if (result.Exception != null)
                    ex = new AggregateException(Utilities.ResourceStrings.AggregateException, result.Exception, ex);

                return new Result(ResultCode.Exception, exception: ex);
            }
            finally
            {
                if (sendsAcquired)
                    packet.Connection.ReleaseSends();

                if (connectionSendsAcquired != null)
                    foreach (var connection in connectionSendsAcquired)
                        connection.ReleaseSends();

                //packet.Locker.Exchange(0);
                if (packet.Locker.CompareExchange(0, 1) != 1)
                    throw new InvalidOperationException("Fatal: Packet Locker");
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
