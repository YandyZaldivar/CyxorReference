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
    using Extensions;
    using Serialization;

    partial class Frame
    {
        Node Node;
        Client Client;

        internal int Id { get; set; }
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
        internal Opcode Opcode { get; set; }
        internal Serializer Payload { get; private set; }
        internal PacketPriority Priority { get; set; }
        internal PacketQueryMode QueryMode { get; set; }

        Serializer OutHeader;
        Serializer OutPayload;

        internal bool IsQueryAndReply => QueryMode == PacketQueryMode.QueryAndReply;
        internal bool IsQuery => IsQueryAndReply || QueryMode == PacketQueryMode.Query;
        internal bool IsReply => IsQueryAndReply || QueryMode == PacketQueryMode.Reply;

        internal MessageFormat Format
        {
            get => Opcode == Opcode.Binary ? MessageFormat.CyxorBinary : Opcode == Opcode.Text ? MessageFormat.JsonText : MessageFormat.None;
            set => Opcode = value == MessageFormat.CyxorBinary ? Opcode.Binary : value == MessageFormat.JsonText ? Opcode.Text : Opcode;
        }

        //bool File;

        internal Frame(Node node)
        {
            Node = node;
            Client = node.AsClient;
            //Server = node as Server;

            OutPayload = Node.Pools.PopBuffer();
            OutHeader = new Serializer(new byte[64], 0, 64);
        }

        internal IEnumerable<FramePacket> Fragments
        {
            get
            {
                var protocolFrame = new ProtocolFrame(Node, true, Opcode, OutPayload.Length);

                if (protocolFrame.Mask)
                    protocolFrame.TryApplyMask(new ArraySegment<byte>(OutPayload.Buffer, 0, OutPayload.Length));

                var encryptFallbackSpace = Encrypt ? 160 : 0;

                protocolFrame.Fin = protocolFrame.Size + OutPayload.Length < Node.Config.IOBufferSize - encryptFallbackSpace;

                if (protocolFrame.Fin)
                {
                    OutHeader.Reset();
                    protocolFrame.Serialize(OutHeader);

                    var framePacket = new FramePacket
                    {
                        Header = new ArraySegment<byte>(OutHeader.Buffer, 0, OutHeader.Length),
                        Payload = new ArraySegment<byte>(OutPayload.Buffer, 0, OutPayload.Length)
                    };

                    yield return framePacket;
                }
                else
                {
                    for (var remainingBytes = OutPayload.Length; remainingBytes > 0; remainingBytes -= protocolFrame.PayloadLength)
                    {
                        if (remainingBytes != OutPayload.Length)
                            protocolFrame.Opcode = Opcode.Continuation;

                        protocolFrame.PayloadLength = Node.Config.IOBufferSize - encryptFallbackSpace - protocolFrame.Size;
                        protocolFrame.PayloadLength = protocolFrame.PayloadLength <= remainingBytes ? protocolFrame.PayloadLength : remainingBytes;

                        if (protocolFrame.PayloadLength == remainingBytes)
                            protocolFrame.Fin = true;

                        OutHeader.Reset();
                        protocolFrame.Serialize(OutHeader);

                        var framePacket = new FramePacket
                        {
                            Header = new ArraySegment<byte>(OutHeader.Buffer, 0, OutHeader.Length),
                            Payload = new ArraySegment<byte>(OutPayload.Buffer, OutPayload.Length - remainingBytes, OutPayload.Length)
                        };

                        yield return framePacket;
                    }
                }
            }
        }

        void Serialize()
        {

        }

        /*
        static int TryReadProtocol(Connection connection, Serializer serializer, ref int payloadLength, ref Opcode opcode, ref bool fin)
        {
            var server = connection.Node as Server;
            var startPosition = serializer.Position;

            var b = default(byte);
            if (!serializer.TryReadByte(out b))
                return FrameRead.Broken;

            var bb = (BitBuffer)b;

            fin = bb[ProtocolMap.Fin];
            opcode = (Opcode)bb.Read(ProtocolMap.Opcode, 4);

            if (!fin && (opcode == Opcode.Close || opcode == Opcode.Ping || opcode == Opcode.Pong))
                return FrameRead.Error;

            if (!serializer.TryReadByte(out b))
                return FrameRead.Broken;

            bb = b;

            var mask = bb[ProtocolMap.Mask];

            if (!mask && server != null || mask && server == null)
                return FrameRead.Error;

            payloadLength = (int)bb.Read(ProtocolMap.PayloadLenght, 7);

            if (payloadLength == 126)
            {
                var ushortLength = default(ushort);

                if (!serializer.TryReadUInt16(out ushortLength))
                    return FrameRead.Broken;

                payloadLength = ushortLength;
            }
            else if (payloadLength == 127)
            {
                var ulongLength = default(ulong);

                if (!serializer.TryReadUInt64(out ulongLength))
                    return FrameRead.Broken;

                if (ulongLength > int.MaxValue)
                    return FrameRead.Error;

                payloadLength = (int)ulongLength;
            }

            serializer.SetPosition(serializer.Position + (mask ? 4 : 0));

            if (payloadLength > serializer.Length - serializer.Position)
                return (serializer.Position - startPosition) + payloadLength;

            if (mask)
            {
                for (int i = serializer.Position, j = 0; i < serializer.Position + payloadLength; i++, j++)
                    serializer.Buffer[i] = (byte)(serializer.Buffer[i] ^ serializer.Buffer[(i - 4) + (j % 4)]);
            }

            return FrameRead.Complete;
        }

        static int TryReadSubProtocol(Connection connection, Serializer serializer, Frame frame)
        {
            var server = connection.Node as Server;

            var b = default(byte);

            if (!serializer.TryReadByte(out b))
                return FrameRead.Error;

            var bb = (BitBuffer)b;

            if (bb[ProtocolMap.CfgByte2])
            {
                if (!serializer.TryReadByte(out b))
                    return FrameRead.Error;

                var bb2 = (BitBuffer)b;

                frame.Anonymous = bb2[ProtocolMap.Anonymous];
                frame.Progress = bb2[ProtocolMap.Progress];
                frame.Compress = bb2[ProtocolMap.Compress];
                frame.Priority = (PacketPriority)bb2.Read(ProtocolMap.Priority, 2);

                if (bb2[ProtocolMap.CfgByte3])
                    return FrameRead.Error;
            }

            frame.Internal = bb[ProtocolMap.Internal];
            frame.Broadcast = bb[ProtocolMap.Broadcast];

            var id = 0;
            if (bb[ProtocolMap.Type])
                if (!serializer.TryReadInt32(out id))
                    return FrameRead.Error;
            frame.Id = id;

            frame.QueryMode = (PacketQueryMode)bb.Read(ProtocolMap.QueryMode, 2);

            var queryId = 0;
            if (frame.IsQuery)
                if (!serializer.TryReadInt32(out queryId))
                    return FrameRead.Error;
            frame.QueryId = queryId;

            var replyId = 0;
            if (frame.IsReply)
                if (!serializer.TryReadInt32(out replyId))
                    return FrameRead.Error;
            frame.ReplyId = replyId;

            if (server != null)
                if (bb[ProtocolMap.Sender])
                    return FrameRead.Error;

            if (!bb[ProtocolMap.Address] && frame.Broadcast)
            {
                if (server == null)
                    return FrameRead.Error;

                frame.RelayConnection = connection;

                frame.Connections = from cx in server.Connections.List
                                    where cx != connection
                                    select cx;
            }
            else if (bb[ProtocolMap.Address])
            {
                uint addressHashCode;
                if (!serializer.TryReadUInt32(out addressHashCode))
                    return FrameRead.Error;

                // TODO: Handle the case when address be null

                if (server == null)
                {
                    if (!frame.Broadcast || (frame.Broadcast && (!frame.Anonymous && !bb[ProtocolMap.Sender])))
                        return FrameRead.Error;

                    // TODO:
                    //address = Link.FindGroup((int)receiverHashCode);
                    frame.Address = string.Empty;
                }
                else
                {
                    frame.RelayConnection = connection;

                    if (!frame.Broadcast)
                        connection = server.Connections.Find((int)addressHashCode);
                    else
                    {
                        // TODO: Select correct group connections
                        //var group = server.GetGroup((int)addressHashCode);
                        frame.Address = string.Empty;

                        frame.Connections = from cx in server.Connections.List
                                            where cx != connection
                                            select cx;
                    }

                    if (!frame.Anonymous)
                        frame.Sender = connection.Name;
                }
            }

            var sender = default(string);
            if (bb[ProtocolMap.Sender])
                if (!serializer.TryReadString(out sender, throwOnEncodingException: false))
                    return FrameRead.Error;
            frame.Sender = sender;

            return FrameRead.Complete;
        }
        */

        internal static int TryRead(Connection connection, Serializer serializer, out Frame frame)
        {
            frame = default(Frame);

            var node = connection.Node;
            var frameRead = FrameResult.Ok;
            var frames = connection.Link.Receives.Frames;

            //var fin = false;
            //var payloadLength = 0;
            //var opcode = default(Opcode);

            var protocol = new ProtocolFrame(node);
            var subprotocol = new SubprotocolFrame(node);

            protocol.Deserialize(serializer);

            if ((frameRead = protocol.Result) != FrameResult.Ok)
                return frameRead;

            var channel = 0;
            var payloadLength = protocol.PayloadLength;
            var payloadLenghtStartPosition = serializer.Position;

            if (!protocol.Fin)
            {
                if (!serializer.TryDeserializeInt32(out channel))
                    return FrameResult.Error;
            }

            payloadLength = payloadLength - (serializer.Position - payloadLenghtStartPosition);

            var thisFrame = default(Frame);

            if (protocol.Opcode == Opcode.Continuation)
            {
                if (!frames.TryGetValue(channel, out thisFrame))
                    return FrameResult.Error;

                if (protocol.Fin)
                {
                    frame = thisFrame;
                    frames.Remove(channel);
                }
            }
            else
            {
                thisFrame = new Frame(node);

                var length = -1;

                if (protocol.Fin)
                    frame = thisFrame;
                else
                {
                    thisFrame.Opcode = protocol.Opcode;
                    thisFrame.Channel = channel;

                    frames.Add(channel, thisFrame);

                    if (!serializer.TryDeserializeInt32(out length))
                        return FrameResult.Error;
                }

                if ((frameRead = subprotocol.TryReadSubProtocol(connection, serializer)) != FrameResult.Ok)
                    return frameRead;

                payloadLength = payloadLength - (serializer.Position - payloadLenghtStartPosition);

                if (payloadLength > 0)
                {
                    thisFrame.Payload = node.Pools.PopBuffer();
                    thisFrame.Payload.EnsureCapacity(length != -1 ? length : payloadLength);
                }
            }

            thisFrame.Payload?.SerializeRaw(serializer.Buffer, serializer.Position, payloadLength);
            serializer.Position = serializer.Position + payloadLength;

            if (!protocol.Fin)
                return FrameResult.Partial;
            else if (frame.Payload?.Position != frame.Payload?.Length)
                return FrameResult.Error;

            if (frame.Payload != null)
            {
                //if (frame.Internal && frame.Id == (int)InternalCoreApiId.Encrypted)
                if (frame.Internal)
                {
                    //if (connection?.Link?.Crypto == null)
                    //    throw new CryptographicException("Cyxor cryptographic exception: Null cryptographic algorithm.");

                    serializer = connection.Link.Crypto.Decrypt(frame.Payload.Buffer, 0, frame.Payload.Length);
                    node.Pools.PushBuffer(frame.Payload);
                    frame.Payload = null;

                    if ((frameRead = subprotocol.TryReadSubProtocol(connection, serializer)) != FrameResult.Ok)
                        return frameRead;

                    payloadLength = serializer.Length - serializer.Position;

                    if (payloadLength > 0)
                    {
                        frame.Payload = node.Pools.PopBuffer();
                        frame.Payload.EnsureCapacity(payloadLength);
                        frame.Payload?.SerializeRaw(serializer.Buffer, serializer.Position, payloadLength);
                        node.Pools.PushBuffer(serializer);
                    }
                }

                if (frame.Compress)
                {
                    if (frame.Payload == null)
                        return FrameResult.Error;

                    if (frame.Compress)
                    {
                        var memoryStream = new MemoryStream();
                        // TODO: Reuse the compressStream?
                        using (var compressStream = new GZipStream(memoryStream, CompressionMode.Decompress, leaveOpen: true))
                            compressStream.Write(frame.Payload.Buffer, 0, frame.Payload.Length);

                        node.Pools.PushBuffer(frame.Payload);
                        frame.Payload = new Serializer(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                    }
                }
            }

            return FrameResult.Ok;
        }

        internal void Reset()
        {
            OutHeader.Reset();
            Payload.Reset(Node);
            OutPayload.Reset(Node);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
