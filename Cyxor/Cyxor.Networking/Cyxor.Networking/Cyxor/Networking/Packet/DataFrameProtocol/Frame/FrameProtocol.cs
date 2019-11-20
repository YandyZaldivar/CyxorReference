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
    using Serialization;

    /// <summary>
    /// Implementation of the WebSocket protocol bytes structure
    /// https://tools.ietf.org/html/rfc6455.html, including
    /// masking <see cref="TryApplyMask(ArraySegment{byte})"/>,
    /// size <see cref="HeaderSize"/>,
    /// serialization <see cref="Serialize(Serializer)"/>,
    /// and deserialization <see cref="Deserialize(Serializer)"/>.
    /// </summary>
    /// <remarks>
    /// 0                   1                   2                   3
    /// 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    /// +-+-+-+-+-------+-+-------------+-------------------------------+
    /// |F|R|R|R| opcode|M| Payload len |    Extended payload length    |
    /// |I|S|S|S|  (4)  |A|     (7)     |             (16/64)           |
    /// |N|V|V|V|       |S|             |   (if payload len==126/127)   |
    /// | |1|2|3|       |K|             |                               |
    /// +-+-+-+-+-------+-+-------------+ - - - - - - - - - - - - - - - +
    /// |     Extended payload length continued, if payload len == 127  |
    /// + - - - - - - - - - - - - - - - +-------------------------------+
    /// |                               |Masking-key, if MASK set to 1  |
    /// +-------------------------------+-------------------------------+
    /// | Masking-key(continued)        |          Payload Data         |
    /// +-------------------------------- - - - - - - - - - - - - - - - +
    /// :                     Payload Data continued...                 :
    /// + - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
    /// |                     Payload Data continued...                 |
    /// +---------------------------------------------------------------+
    /// </remarks>
    struct FrameProtocol : ISerializable
    {
        readonly Node Node;

        public bool Fin { get; set; }
        public bool Mask { get; set; }
        public Opcode Opcode { get; set; }
        public int MaskingKey { get; set; }
        public long PayloadLength { get; set; }

        public int Result { get; private set; }

        public FrameProtocol(Node node) : this(node, false, default, 0) { }

        public FrameProtocol(Node node, bool fin, Opcode opcode, long payloadLength)
        {
            Node = node;

            Fin = fin;
            MaskingKey = 0;
            Opcode = opcode;
            Mask = node.IsClient;
            PayloadLength = payloadLength;

            if (Mask)
            {
                if (Node.Config.FrameRNG)
                    MaskingKey = node.Random.Next();
                else unsafe
                {
                    var bytes = new byte[4];
                    node.Rng.GetBytes(bytes);

                    fixed (byte* ptr = bytes)
                        MaskingKey = *(int*)ptr;
                }
            }

            Result = FrameResult.Ok;
        }

        /// <summary>
        /// The maximum possible size of the header (14 bytes).
        /// </summary>
        public const int MaxHeaderSize = 2 + 8 + 4;

        /// <summary>
        /// The size in bytes used by the header.
        /// </summary>
        public int HeaderSize
        {
            get
            {
                var size = 2;

                if (PayloadLength > 125)
                    if (PayloadLength <= ushort.MaxValue)
                        size += 2;
                    else
                        size += 8;

                if (Mask)
                    size += 4;

                return size;
            }
        }

        public bool TryApplyMask(ArraySegment<byte> arraySegment)
        {
            unsafe
            {
                if (!Mask)
                    return false;

                var key = MaskingKey;
                var keyPtr = (byte*)&key;

                for (int i = arraySegment.Offset, j = 0; j < arraySegment.Count; i++, j++)
                    //arraySegment.Array[i] = (byte)(arraySegment.Array[i] ^ ptr[j % 4]);
                    arraySegment.Array[i] = (byte)(arraySegment.Array[i] ^ *(keyPtr + j % 4));

                return true;
            }
        }

        /// <summary>
        /// Serialize this WebSocket protocol frame into <paramref name="serializer"/>
        /// </summary>
        /// <remarks>
        /// The structure of the basic WebSocket protocol is composed by two bytes,
        /// followed optionally by 0, 2 or 8 bytes depending on the payload length,
        /// and optionally 4 more bytes if a mask is present.
        /// 
        /// First byte bits:
        ///     1 - Fin
        ///     2 - Rsv1
        ///     3 - Rsv2
        ///     4 - Rsv3
        ///     5 - Opcode
        ///     6 - Opcode
        ///     7 - Opcode
        ///     8 - Opcode
        ///     
        /// The Opcode is defined in <see cref="Networking.Opcode"/>.
        ///     
        /// Second byte bits:
        ///     1 - Mask
        ///     2 - PayloadLength
        ///     3 - PayloadLength
        ///     4 - PayloadLength
        ///     5 - PayloadLength
        ///     6 - PayloadLength
        ///     7 - PayloadLength
        ///     8 - PayloadLength
        ///   
        /// Payload rules:
        /// 
        /// - If the payload length is less or equal than 125 the it is stored in those 7 bits.
        /// - If the payload length is less or equal than <see cref="ushort.MaxValue"/>,
        /// then 126 is stored and the following two bytes will contain the payload length.
        /// - If the payload length is greater than <see cref="ushort.MaxValue"/>,
        /// then 127 is stored and the following 8 bytes will contain the payload length.
        /// 
        /// Finally if this is a client implementation and contains a <see cref="Mask"/>,
        /// the <see cref="MaskingKey"/> is stored in the next 4 bytes.
        /// </remarks>
        /// <param name="serializer"></param>
        public void Serialize(Serializer serializer)
        {
            var bits = (BitSerializer)0;
            bits[FrameProtocolMap.Fin] = Fin;
            bits.Serialize(value: (long)Opcode, offset: FrameProtocolMap.Opcode);

            serializer.Serialize(bits);

            bits = 0;
            bits[FrameProtocolMap.Mask] = Mask;

            if (PayloadLength <= 125)
                bits.Serialize(PayloadLength, offset: FrameProtocolMap.PayloadLength);
            else if (PayloadLength <= ushort.MaxValue)
                bits.Serialize(value: 126, offset: FrameProtocolMap.PayloadLength);
            else
                bits.Serialize(value: 127, offset: FrameProtocolMap.PayloadLength);

            serializer.Serialize(bits);

            if (PayloadLength > 125)
                if (PayloadLength <= ushort.MaxValue)
                    serializer.Serialize((ushort)PayloadLength);
                else
                    serializer.Serialize((ulong)PayloadLength);

            if (Mask)
                serializer.Serialize(MaskingKey);
        }

        /// <summary>
        /// Deserialize a WebSocket protocol header frame from <paramref name="serializer"/>.
        /// </summary>
        /// <remarks>
        /// Only the header is deserialized into this protocol frame.
        /// The payload needs further processing.
        /// </remarks>
        public void Deserialize(Serializer serializer)
        {
            var server = Node as Server;
            var startPosition = serializer.Int32Position;

            if (!serializer.TryDeserializeByte(out var b))
            {
                Result = FrameResult.Header;
                return;
            }

            var bits = (BitSerializer)b;

            Fin = bits[FrameProtocolMap.Fin];
            Opcode = (Opcode)bits.Deserialize(FrameProtocolMap.Opcode, 4);

            if (!Fin && Opcode >= Opcode.Close)
            {
                Result = FrameResult.Error;
                return;
            }

            if (!serializer.TryDeserializeByte(out b))
            {
                Result = FrameResult.Header;
                return;
            }

            bits = b;

            Mask = bits[FrameProtocolMap.Mask];

            if (!Mask && server != null || Mask && server == null)
            {
                Result = FrameResult.Error;
                return;
            }

            PayloadLength = bits.Deserialize(FrameProtocolMap.PayloadLength, 7);

            if (PayloadLength == 126)
            {
                if (!serializer.TryDeserializeUInt16(out var ushortLength))
                {
                    Result = FrameResult.Header;
                    return;
                }

                PayloadLength = ushortLength;
            }
            else if (PayloadLength == 127)
            {
                if (!serializer.TryDeserializeInt64(out var longLength))
                {
                    Result = FrameResult.Header;
                    return;
                }

                // TODO: Consider doing size check in the Frame class
                if (longLength > Node.Config.Packets.MaxSize - MaxHeaderSize)
                {
                    Result = FrameResult.Error;
                    return;
                }

                PayloadLength = longLength;
            }

            if (Mask)
            {
                if (!serializer.TryDeserializeUInt32(out var maskingKey))
                {
                    Result = FrameResult.Header;
                    return;
                }

                MaskingKey = (int)maskingKey;
            }

            //if (PayloadLength > serializer.Count)
            //{
            //    // TODO: Review this
            //    Result = (serializer.Int32Position - startPosition) + PayloadLength;
            //    return;
            //}

            //TryApplyMask(new ArraySegment<byte>(serializer.Buffer, serializer.Int32Position, PayloadLength));

            Result =  FrameResult.Ok;
            return;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
