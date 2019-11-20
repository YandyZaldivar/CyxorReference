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
using System.IO.Compression;
using System.Collections.Generic;

namespace Cyxor.Networking
{
    using Serialization;

    struct FrameSubprotocol
    {
        readonly Node Node;
        Connection RelayConnection;
        IEnumerable<Connection> Connections;

        internal int Id { get; set; }
        internal int Module { get; set; }
        internal int QueryId { get; set; }
        internal int ReplyId { get; set; }
        internal int Channel { get; set; }
        internal bool Encrypt { get; set; }
        internal bool Progress { get; set; }
        internal bool Compress { get; set; }
        internal bool Internal { get; set; }
        internal bool Broadcast { get; set; }
        internal bool Anonymous { get; set; }
        internal string Sender { get; private set; }
        internal string Address { get; set; }
        //internal Opcode Opcode { get; set; }
        internal Serializer Payload { get; private set; }
        internal PacketPriority Priority { get; set; }
        internal PacketQueryMode QueryMode { get; set; }

        //internal ProtocolFrame ProtocolFrame { get; set; }

        internal bool IsQueryAndReply => QueryMode == PacketQueryMode.QueryAndReply;
        internal bool IsQuery => IsQueryAndReply || QueryMode == PacketQueryMode.Query;
        internal bool IsReply => IsQueryAndReply || QueryMode == PacketQueryMode.Reply;

        bool IsSubprotocolConfigByte1 => true;
        bool IsSubprotocolConfigByte2 => Anonymous || Progress || Compress || Priority != PacketPriority.Exclusive || IsSubprotocolConfigByte3;
        bool IsSubprotocolConfigByte3 => false;

        public FrameSubprotocol(Node node)
        {
            Node = node;
            Connections = null;
            RelayConnection = null;

            Id = 0;
            Module = 0;
            QueryId = 0;
            ReplyId = 0;
            Channel = 0;
            Payload = null;
            Encrypt = false;
            Progress = false;
            Compress = false;
            Internal = false;
            Broadcast = false;
            Anonymous = false;
            Sender = default(string);
            Address = default(string);
            Priority = PacketPriority.Exclusive;
            QueryMode = PacketQueryMode.Send;
        }

        byte GetSubprotocolConfigByte1()
        {
            var bits = (BitSerializer)0;

            bits[0] = Internal;
            bits[1] = Module != 0;
            bits.Serialize((int)QueryMode, FrameProtocolMap.QueryMode);
            bits[4] = Broadcast;
            bits[5] = !string.IsNullOrEmpty(Address);
            bits[6] = RelayConnection != null && !Anonymous;
            bits[7] = IsSubprotocolConfigByte2;

            return bits;
        }

        byte GetSubprotocolConfigByte2()
        {
            if (!IsSubprotocolConfigByte2)
                return 0;

            var bits = (BitSerializer)0;

            bits[0] = Anonymous;
            bits[1] = Progress;
            bits[2] = Compress;
            bits.Serialize((int)Priority, FrameProtocolMap.Priority);
            bits[7] = IsSubprotocolConfigByte3;

            return bits;
        }

        public void Serialize(Serializer serializer)
        {
            //var serializer = Node.Pools.PopBuffer();

            serializer.Serialize(GetSubprotocolConfigByte1());

            if (IsSubprotocolConfigByte2)
            {
                serializer.Serialize(GetSubprotocolConfigByte2());

                if (IsSubprotocolConfigByte3)
                    throw new NotImplementedException();
            }

            if (Module != 0)
                serializer.Serialize(Module);

            serializer.Serialize(Id);

            if (IsQuery)
                serializer.Serialize(QueryId);

            if (IsReply)
                serializer.Serialize(ReplyId);

            if (!string.IsNullOrEmpty(Address))
                serializer.Serialize((uint)Utilities.HashCode.GetFrom(Address));

            if (RelayConnection != null && !Anonymous)
                serializer.Serialize(RelayConnection.Name);

            if (Payload != null && Payload.Int32Length > 0)
            {
                serializer.EnsureCapacity(Payload.Int32Length + 128);

                if (Compress)
                {
                    var compressStartPosition = serializer.Int32Position;

                    var memoryStream = new MemoryStream(serializer.Buffer, serializer.Int32Position, Payload.Int32Length + 128);

                    using (var compressStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
                    {
                        compressStream.Write(Payload.Buffer, 0, Payload.Int32Length);
                        compressStream.Flush();

                        if (memoryStream.Length - serializer.Int32Position > Payload.Int32Length)
                            Compress = false;
                        else
                        {
                            serializer.SetLength((int)memoryStream.Length - serializer.Int32Position);
                            serializer.Int32Position = serializer.Int32Length;
                        }
                    }
                }

                if (!Compress)
                    serializer.SerializeRaw(Payload);
            }
        }

        public int TryReadSubProtocol(Connection connection, Serializer serializer)
        {
            var server = connection.Node as Server;

            if (!serializer.TryDeserializeByte(out var b))
                return FrameResult.Error;

            var bits1 = (BitSerializer)b;

            if (bits1[FrameProtocolMap.CfgByte2])
            {
                if (!serializer.TryDeserializeByte(out b))
                    return FrameResult.Error;

                var bits2 = (BitSerializer)b;

                Anonymous = bits2[FrameProtocolMap.Anonymous];
                Progress = bits2[FrameProtocolMap.Progress];
                Compress = bits2[FrameProtocolMap.Compress];
                Priority = (PacketPriority)bits2.Deserialize(FrameProtocolMap.Priority, 2);

                if (bits2[FrameProtocolMap.CfgByte3])
                    return FrameResult.Error;
            }

            Internal = bits1[FrameProtocolMap.Encrypted];
            Broadcast = bits1[FrameProtocolMap.Broadcast];

            if (bits1[FrameProtocolMap.Command])
            {
                if (!serializer.TryDeserializeInt32(out var module))
                    return FrameResult.Error;
                Module = module;
            }

            if (!serializer.TryDeserializeInt32(out var id))
                return FrameResult.Error;
            Id = id;

            QueryMode = (PacketQueryMode)bits1.Deserialize(FrameProtocolMap.QueryMode, 2);

            if (IsQuery)
            {
                if (!serializer.TryDeserializeInt32(out var queryId))
                    return FrameResult.Error;
                QueryId = queryId;
            }

            if (IsReply)
            {
                if (!serializer.TryDeserializeInt32(out var replyId))
                    return FrameResult.Error;
                ReplyId = replyId;
            }

            if (server != null)
                if (bits1[FrameProtocolMap.Sender])
                    return FrameResult.Error;

            if (!bits1[FrameProtocolMap.Address] && Broadcast)
            {
                if (server == null)
                    return FrameResult.Error;

                RelayConnection = connection;

                Connections = from cx in server.Connections.List where cx != connection select cx;
            }
            else if (bits1[FrameProtocolMap.Address])
            {
                if (!serializer.TryDeserializeUInt32(out var addressHashCode))
                    return FrameResult.Error;

                // TODO: Handle the case when address be null

                if (server == null)
                {
                    if (!Broadcast || (Broadcast && (!Anonymous && !bits1[FrameProtocolMap.Sender])))
                        return FrameResult.Error;

                    // TODO:
                    //address = Link.FindGroup((int)receiverHashCode);
                    Address = string.Empty;
                }
                else
                {
                    RelayConnection = connection;

                    if (!Broadcast)
                        connection = server.Connections.Find((int)addressHashCode);
                    else
                    {
                        // TODO: Select correct group connections
                        //var group = server.GetGroup((int)addressHashCode);
                        Address = string.Empty;

                        Connections = from cx in server.Connections.List
                                            where cx != connection
                                            select cx;
                    }

                    if (!Anonymous)
                        Sender = connection.Name;
                }
            }

            if (bits1[FrameProtocolMap.Sender])
            {
                if (!serializer.TryDeserializeString(out var sender, throwOnEncodingException: false))
                    return FrameResult.Error;
                Sender = sender;
            }

            return FrameResult.Ok;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
