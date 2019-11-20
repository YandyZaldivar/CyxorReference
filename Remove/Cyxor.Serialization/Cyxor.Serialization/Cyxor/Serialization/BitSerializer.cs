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

namespace Cyxor.Serialization
{
    public struct BitSerializer : IComparable, IComparable<BitSerializer>, IEquatable<BitSerializer>
    {
        long Bits;
        static readonly long[] Mask;
        public const int Capacity = 64;

        static BitSerializer()
        {
            Mask = new long[Capacity];

            Mask[0] = 1;

            for (var i = 0; i < Capacity - 1; i++)
                Mask[i + 1] = Mask[i] * 2;
        }

        //public BitSerializer(long value) => Bits = value;

        public BitSerializer(long value) { Bits = value; }

        public static implicit operator long(BitSerializer value) => value.Bits;
        public static implicit operator int(BitSerializer value) => (int)value.Bits;
        public static implicit operator byte(BitSerializer value) => (byte)value.Bits;
        public static implicit operator short(BitSerializer value) => (short)value.Bits;

        public static implicit operator BitSerializer(int value) => new BitSerializer(value);
        public static implicit operator BitSerializer(byte value) => new BitSerializer(value);
        public static implicit operator BitSerializer(long value) => new BitSerializer(value);
        public static implicit operator BitSerializer(short value) => new BitSerializer(value);

        public override int GetHashCode() => Bits.GetHashCode();

        public static bool operator ==(BitSerializer value1, BitSerializer value2) => value1.Bits == value2.Bits;
        public static bool operator !=(BitSerializer value1, BitSerializer value2) => value1.Bits != value2.Bits;

        public bool Equals(BitSerializer obj) => Bits == obj.Bits;
        public int CompareTo(BitSerializer value) => Bits.CompareTo(value.Bits);

        public override bool Equals(object obj)
        {
            if (!(obj is BitSerializer))
                return false;

            return Bits == ((BitSerializer)obj).Bits;
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            if (value is BitSerializer)
                return Bits.CompareTo(((BitSerializer)value).Bits);

            throw new ArgumentException($"Argument must be a {nameof(BitSerializer)}.", nameof(value));
        }

        public bool this[int index]
        {
            get => (Bits & Mask[index]) == Mask[index];
            set => Bits = value ? Bits | Mask[index] : (Bits & Mask[index]) == Mask[index] ? Bits ^ Mask[index] : Bits;
        }

        public int Count
        {
            get
            {
                var count = 1;
                var value = Bits;

                while ((value >>= 1) != 0)
                    count++;

                return count;
            }
        }

        public long Deserialize(int offset, int count)
        {
            if (offset == 0 && count == 0)
                return Bits;

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), Utilities.ResourceStrings.ExceptionNegativeNumber);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), Utilities.ResourceStrings.ExceptionNegativeNumber);

            if (Capacity - offset < count)
                throw new ArgumentException("The values provided exceed the capacity of the BitSerializer.");

            var result = 0;

            for (var i = 0; i < count; i++)
                result += this[i + offset] == true ? (int)(1 * Math.Pow(2, i)) : 0;

            return result;
        }

        public int Serialize(long value, int offset)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), Utilities.ResourceStrings.ExceptionNegativeNumber);

            var count = Utilities.Bits.Required(value);

            if (Capacity - offset < count)
                throw new ArgumentException("The values provided exceed the capacity of the BitBuffer.");

            var valueBits = (BitSerializer)value;

            for (var i = 0; i < count; i++)
                this[i + offset] = valueBits[i];

            return count;
        }

        public override string ToString()
        {
            var bitCount = Count;
            var bitString = bitCount == 1 ? "bit" : "bits";

            return $"{Bits} [{bitCount}{bitString}] {{{Convert.ToString(Bits, 2)}}}";
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
