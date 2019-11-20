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
using System.Text;
using System.Numerics;
using System.Security;
using System.Reflection;
using System.Security.Cryptography;

#if !NET20
using System.Linq;
#endif

namespace Cyxor.Security
{
    public static class Utilities
    {
#if NET20 || NET35
        public static class String
        {
            public static bool IsNullOrWhiteSpace(string value)
            {
                if (value == null)
                    return true;

                for (int i = 0; i < value.Length; i++)
                    if (!Char.IsWhiteSpace(value[i]))
                        return false;

                return true;
            }
        }
#endif

        public static class Reflection
        {
            public static TAttribute GetCustomAssemblyAttribute<TAttribute>(Type type) where TAttribute : Attribute
#if NET20
            {
                var attributes = type.Assembly.GetCustomAttributes(typeof(TAttribute), inherit: false);

                if (attributes.Length > 0)
                    return (TAttribute)attributes[0];

                return default(TAttribute);
            }
#elif NET40 || NET35
                => (TAttribute)type.Assembly.GetCustomAttributes(typeof(TAttribute), inherit: false).FirstOrDefault();
#else
                => type.GetTypeInfo().Assembly.GetCustomAttribute<TAttribute>();
#endif
        }

        public static class Marshal
        {
            public static IntPtr SecureStringToCoTaskMemUnicode(SecureString value) =>
#if NETSTANDARD1_0 || NETSTANDARD1_3
            System.Security.SecureStringMarshal.SecureStringToCoTaskMemUnicode(value);
#else
            System.Runtime.InteropServices.Marshal.SecureStringToCoTaskMemUnicode(value);
#endif

            public static void ZeroFreeCoTaskMemUnicode(IntPtr value) =>
#if NETSTANDARD1_0
            System.Security.SecureStringMarshal.ZeroFreeCoTaskMemUnicode(value);
#else
            System.Runtime.InteropServices.Marshal.ZeroFreeCoTaskMemUnicode(value);
#endif
        }

        public static class Converter
        {
            public static string FromSecureString(SecureString value)
            {
                if (value == null || value.Length == 0)
                    return default(string);

                var result = (string)null;

                var charPtr = Marshal.SecureStringToCoTaskMemUnicode(value);
                unsafe { result = new string((char*)charPtr, 0, value.Length); }
                Marshal.ZeroFreeCoTaskMemUnicode(charPtr);

                return result;
            }

            public static SecureString ToSecureString(string value)
            {
                if (string.IsNullOrEmpty(value))
                    return null;

                var secureString = (SecureString)null;

                unsafe
                {
                    fixed (char* charPtr = value)
                        secureString = new SecureString(charPtr, value.Length);
                }

                secureString.MakeReadOnly();
                return secureString;
            }
        }

        internal static class Crypto
        {
            internal static BigInteger Random(RandomNumberGenerator algorithm, int bitCount)
            {
                int byteCount = bitCount / 8;

                var bytes = new byte[byteCount + 1];
                algorithm.GetBytes(bytes);

                bytes[byteCount] = 0;

                return new BigInteger(bytes);
            }

            internal static byte[] Pad(byte[] value, int length)
            {
                if (value.Length < length)
                {
                    var bytes = new byte[length];
                    Buffer.BlockCopy(value, 0, bytes, length - value.Length, value.Length);
                    return bytes;
                }

                return value;
            }

            internal static BigInteger Hash(BigInteger value, BigInteger N, HashAlgorithm algorithm)
            {
                var hash = algorithm.ComputeHash(value.ToByteArray());
                var bytes = new byte[hash.Length + 1];
                Buffer.BlockCopy(hash, 0, bytes, 0, hash.Length);
                bytes[hash.Length] = 0;

                return new BigInteger(bytes) % N;
            }

            internal static BigInteger HashPair(BigInteger value1, BigInteger value2, BigInteger N, HashAlgorithm algorithm, bool padded = false, int padLength = 0)
            {
                var bytes1 = value1.ToByteArray();
                var bytes2 = value2.ToByteArray();

                if (padded)
                {
                    bytes1 = Pad(bytes1, padLength);
                    bytes2 = Pad(bytes2, padLength);
                }

                var bytes = new byte[bytes1.Length + bytes2.Length + 1];

                Buffer.BlockCopy(bytes1, 0, bytes, 0, bytes1.Length);
                Buffer.BlockCopy(bytes2, 0, bytes, bytes1.Length, bytes2.Length);
                bytes[bytes.Length - 1] = 0;

                return Hash(new BigInteger(bytes), N, algorithm);
            }

            internal static byte[] GetLoginBytes(string username, SecureString password)
            {
                var passwordChars = new char[password.Length];
                var passwordPtr = Marshal.SecureStringToCoTaskMemUnicode(password);

                unsafe
                {
                    for (int i = 0; i < password.Length; i++)
                        passwordChars[i] = *((char*)passwordPtr + i);
                }

                var usernameBytes = Encoding.UTF8.GetBytes(username.ToUpperInvariant());
                var passwordBytes = Encoding.UTF8.GetBytes(passwordChars);

                Marshal.ZeroFreeCoTaskMemUnicode(passwordPtr);
                Array.Clear(passwordChars, 0, passwordChars.Length);

                var loginBytes = new byte[usernameBytes.Length + 1 + passwordBytes.Length];

                Buffer.BlockCopy(usernameBytes, 0, loginBytes, 0, usernameBytes.Length);
                loginBytes[usernameBytes.Length] = (byte)':';
                Buffer.BlockCopy(passwordBytes, 0, loginBytes, usernameBytes.Length + 1, passwordBytes.Length);

                return loginBytes;
            }

            internal static SymmetricAlgorithm CreateSymmetricAlgorithm(SrpSymmetricAlgorithm symmetricAlgorithm)
            {
                switch (symmetricAlgorithm)
                {
#if !NET20
                    case SrpSymmetricAlgorithm.Aes: return Aes.Create();
#else
                    case SrpSymmetricAlgorithm.Rijndael: return Rijndael.Create();
#endif
                    case SrpSymmetricAlgorithm.TripleDES: return TripleDES.Create();

                    default: throw new ArgumentException(nameof(symmetricAlgorithm));
                }
            }
        }

        internal static class ResourceStrings
        {
            internal const string

                ExceptionFormat = "Cyxor..{0}.{1}() : {2}";
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
