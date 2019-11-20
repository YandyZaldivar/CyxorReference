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
using System.Collections;
using System.Globalization;

#if NET20 || NET35 || NETSTANDARD1_0

namespace System.Numerics
{
    public sealed partial class BigInteger
    {
        abstract class Arrays
        {
            internal static int[] Clone(int[] data) => data == null ? null : (int[])data.Clone();
        }

        abstract class Platform
        {
            internal static IList CreateArrayList() => new System.Collections.Generic.List<object>();//new ArrayList();
        }

        public static BigInteger operator +(BigInteger leftSide, BigInteger rightSide) => leftSide.Add(rightSide);
        public static BigInteger operator ^(BigInteger leftSide, BigInteger rightSide) => leftSide.Xor(rightSide);
        public static BigInteger operator %(BigInteger leftSide, BigInteger rightSide) => leftSide.Mod(rightSide);
        public static BigInteger operator /(BigInteger leftSide, BigInteger rightSide) => leftSide.Divide(rightSide);
        public static BigInteger operator -(BigInteger leftSide, BigInteger rightSide) => leftSide.Subtract(rightSide);
        public static BigInteger operator *(BigInteger leftSide, BigInteger rightSide) => leftSide.Multiply(rightSide);
        public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus) => value.ModPow(exponent, modulus);

        //public byte[] ToByteArray()
        //{
        //    return m_digits;
        //}

        public static BigInteger Parse(string value, NumberStyles style)
        {
            if (style != NumberStyles.HexNumber)
                throw new InvalidOperationException();

            return new BigInteger(value, radix: 16);
        }

        public int Sign => SignValue;

        public string ToString(string format)
        {
            if (format != "X")
                throw new InvalidOperationException();

            return ToString(radix: 16);
        }
    }
}

#endif

/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
