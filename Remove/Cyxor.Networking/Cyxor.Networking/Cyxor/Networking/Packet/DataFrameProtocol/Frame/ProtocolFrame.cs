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

    struct ProtocolFrame : ISerializable
    {
        readonly Node Node;

        public bool Fin { get; set; }
        public bool Mask { get; set; }
        public Opcode Opcode { get; set; }
        public int MaskingKey { get; set; }
        public int PayloadLength { get; set; }

        public int Result { get; private set; }

        public ProtocolFrame(Node node) : this(node, false, default, 0) { }

        public ProtocolFrame(Node node, bool fin, Opcode opcode, int payloadLength)
        {
            Node = node;

            Fin = fin;
            MaskingKey = 0;
            Opcode = opcode;
            Mask = node.IsClient;
            PayloadLength = payloadLength;

            if (Mask)
            {
                var bytes = new byte[4];
                node.AsClient.Rng.GetBytes(bytes);

                unsafe
                {
                    fixed (byte* ptr = bytes)
                        MaskingKey = *(int*)ptr;
                }
            }

            Result = FrameResult.Ok;
        }

        public int Size
        {
            get
            {
                var size = 2;

                if (Mask)
                    size += 4;

                if (PayloadLength > 125)
                    if (PayloadLength <= ushort.MaxValue)
                        size += 2;
                    else
                        size += 8;

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

        public void Serialize(Serializer serializer)
        {
            var bits = (BitSerializer)0;
            bits[ProtocolMap.Fin] = Fin;
            bits.Serialize((long)Opcode, ProtocolMap.Opcode);

            serializer.Serialize(bits);

            bits = 0;
            bits[ProtocolMap.Mask] = Mask;

            if (PayloadLength <= 125)
                bits.Serialize(PayloadLength, ProtocolMap.PayloadLength);
            else if ((uint)PayloadLength <= ushort.MaxValue)
                bits.Serialize(126, ProtocolMap.PayloadLength);
            else
                bits.Serialize(127, ProtocolMap.PayloadLength);

            serializer.Serialize(bits);

            if (PayloadLength > 125)
                if ((uint)PayloadLength <= ushort.MaxValue)
                    serializer.Serialize((ushort)PayloadLength);
                else
                    serializer.Serialize((ulong)PayloadLength);

            if (Mask)
                serializer.Serialize(MaskingKey);
        }

        public void Deserialize(Serializer serializer)
        {
            var server = Node as Server;
            var startPosition = serializer.Position;

            if (!serializer.TryDeserializeByte(out var b))
            {
                Result = FrameResult.Broken;
                return;
            }

            var bits = (BitSerializer)b;

            Fin = bits[ProtocolMap.Fin];
            Opcode = (Opcode)bits.Deserialize(ProtocolMap.Opcode, 4);

            if (!Fin && (int)Opcode > 7)
            {
                Result = FrameResult.Error;
                return;
            }

            if (!serializer.TryDeserializeByte(out b))
            {
                Result = FrameResult.Broken;
                return;
            }

            bits = b;

            Mask = bits[ProtocolMap.Mask];

            if (!Mask && server != null || Mask && server == null)
            {
                Result = FrameResult.Error;
                return;
            }

            PayloadLength = (int)bits.Deserialize(ProtocolMap.PayloadLength, 7);

            if (PayloadLength == 126)
            {
                if (!serializer.TryDeserializeUInt16(out var ushortLength))
                {
                    Result = FrameResult.Broken;
                    return;
                }

                PayloadLength = ushortLength;
            }
            else if (PayloadLength == 127)
            {
                if (!serializer.TryDeserializeUInt64(out var ulongLength))
                {
                    Result = FrameResult.Broken;
                    return;
                }

                // TODO: Verify in ws protocol what to do if you receive a not supported length.
                if (ulongLength > int.MaxValue)
                {
                    Result = FrameResult.Error;
                    return;
                }

                PayloadLength = (int)ulongLength;
            }

            if (Mask)
            {
                if (!serializer.TryDeserializeUInt32(out var maskingKey))
                {
                    Result = FrameResult.Broken;
                    return;
                }

                MaskingKey = (int)maskingKey;
            }

            if (PayloadLength > serializer.Count)
            {
                // TODO: Review this
                Result = (serializer.Position - startPosition) + PayloadLength;
                return;
            }

            TryApplyMask(new ArraySegment<byte>(serializer.Buffer, serializer.Position, PayloadLength));

            //if (Mask)
            //{
            //    for (int i = serializer.Position, j = 0; i < serializer.Position + PayloadLength; i++, j++)
            //        serializer.Buffer[i] = (byte)(serializer.Buffer[i] ^ serializer.Buffer[(i - 4) + (j % 4)]);
            //}

            Result =  FrameResult.Ok;
            return;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
