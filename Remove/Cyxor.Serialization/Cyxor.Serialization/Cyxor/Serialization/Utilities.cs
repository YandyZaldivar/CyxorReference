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
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

//#if !NET20
//using System.Linq;
//#endif

#if !BRIDGE_NET
using System.Security;
#endif

namespace Cyxor.Serialization
{
    public class Utilities
    {
        protected Utilities() { }

        public static class Bits
        {
            public static int RequiredBytes(int bits) => (bits + 7) / 8;

#if !BRIDGE_NET

            public static unsafe int Required(int value) => Required((byte*)&value, sizeof(int));
            public static unsafe int Required(long value) => Required((byte*)&value, sizeof(long));
            public static unsafe int Required(char value) => Required((byte*)&value, sizeof(char));
            public static unsafe int Required(byte value) => Required((byte*)&value, sizeof(byte));
            public static unsafe int Required(uint value) => Required((byte*)&value, sizeof(uint));
            public static unsafe int Required(short value) => Required((byte*)&value, sizeof(short));
            public static unsafe int Required(float value) => Required((byte*)&value, sizeof(float));
            public static unsafe int Required(sbyte value) => Required((byte*)&value, sizeof(sbyte));
            public static unsafe int Required(ulong value) => Required((byte*)&value, sizeof(ulong));
            public static unsafe int Required(double value) => Required((byte*)&value, sizeof(double));
            public static unsafe int Required(ushort value) => Required((byte*)&value, sizeof(ushort));

            static unsafe int Required(byte* value, int size)
            {
                var count = 1;

                switch (size)
                {
                    case sizeof(byte): while ((*(byte*)value >>= 1) != 0) count++; break;
                    case sizeof(short): while ((*(ushort*)value >>= 1) != 0) count++; break;
                    case sizeof(int): while ((*(uint*)value >>= 1) != 0) count++; break;
                    case sizeof(long): while ((*(ulong*)value >>= 1) != 0) count++; break;

                    default: throw new ArgumentOutOfRangeException("bits", "Unsupported bits count");
                }

                return count;
            }

#else

            public static int Required(int value) => Required(value, sizeof(int));
            public static int Required(long value) => Required(value, sizeof(long));
            public static int Required(char value) => Required(value, sizeof(char));
            public static int Required(byte value) => Required(value, sizeof(byte));
            public static int Required(uint value) => Required(value, sizeof(uint));
            public static int Required(short value) => Required(value, sizeof(short));
            public static int Required(float value) => Required(value, sizeof(float));
            public static int Required(sbyte value) => Required(value, sizeof(sbyte));
            public static int Required(ulong value) => Required(value, sizeof(ulong));
            public static int Required(double value) => Required(value, sizeof(double));
            public static int Required(ushort value) => Required(value, sizeof(ushort));

            static int Required(ValueType value, int size)
            {
                var count = 1;

                switch (size)
                {
                    case sizeof(byte): var b = (byte)value; while ((b >>= 1) != 0) count++; break;
                    case sizeof(short): var s = (ushort)value; while ((s >>= 1) != 0) count++; break;
                    case sizeof(int): var i = (uint)value; while ((i >>= 1) != 0) count++; break;
                    case sizeof(long): var l = (ulong)value; while ((l >>= 1) != 0) count++; break;

                    default: throw new ArgumentOutOfRangeException("bits", "Unsupported bits count");
                }

                return count;
            }

#endif
        }

        public static class Enum
        {
            public static TEnum GetConstantOrDefault<TEnum>() where TEnum : struct
                => GetConstantOrDefault<TEnum>(nameOrId: null);

            public static TEnum GetConstantOrDefault<TEnum>(string nameOrId) where TEnum : struct
                => nameOrId == null ? System.Enum.GetValues(typeof(TEnum)).Length == 0 ? default(TEnum) :
                (TEnum)System.Enum.GetValues(typeof(TEnum)).GetValue(0) :
#if !BRIDGE_NET
                (TEnum)System.Enum.Parse(typeof(TEnum), nameOrId, ignoreCase: true);
#else
                (TEnum)(object)System.Enum.Parse(typeof(TEnum), nameOrId, ignoreCase: true);
#endif
        }

        public static class Memory
        {
#if BRIDGE_NET
            public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
            {
                for (int i = 0, j = srcOffset, k = dstOffset; i < count; i++, j++, k++)
                    dst[k] = src[k];
            }
#else
            public static unsafe void Memcpy(byte* source, byte* destination, int bytesToCopy)
#if NETSTANDARD1_3
                => System.Buffer.MemoryCopy(source, destination, bytesToCopy, bytesToCopy);
#else
            {
                if (bytesToCopy >= 16)
                {
                    do
                    {
                        *(int*)destination = *(int*)source;
                        *(int*)(destination + 4) = *(int*)(source + 4);
                        *(int*)(destination + 8) = *(int*)(source + 8);
                        *(int*)(destination + 12) = *(int*)(source + 12);

                        destination += 16;
                        source += 16;
                    }
                    while ((bytesToCopy -= 16) >= 16);
                }

                if (bytesToCopy <= 0)
                    return;

                if ((bytesToCopy & 8) != 0)
                {
                    *(int*)destination = *(int*)source;
                    *(int*)(destination + 4) = *(int*)(source + 4);

                    destination += 8;
                    source += 8;
                }

                if ((bytesToCopy & 4) != 0)
                {
                    *(int*)destination = *(int*)source;

                    destination += 4;
                    source += 4;
                }

                if ((bytesToCopy & 2) != 0)
                {
                    *(short*)destination = *(short*)source;

                    destination += 2;
                    source += 2;
                }

                if ((bytesToCopy & 1) == 0)
                    return;

                *destination++ = *source++;
            }
#endif

            public static unsafe void Wstrcpy(char* source, char* destination, int charsToCopy)
#if NETSTANDARD1_3
                => System.Buffer.MemoryCopy(source, destination, charsToCopy * 2, charsToCopy * 2);
#else
            {

                if (charsToCopy <= 0)
                    return;

                if (((int)destination & 2) != 0)
                {
                    *destination = *source;

                    ++destination;
                    ++source;

                    --charsToCopy;
                }

                while (charsToCopy >= 8)
                {
                    *(int*)destination = (int)*(uint*)source;
                    *(int*)(destination + 2) = (int)*(uint*)(source + 2);
                    *(int*)(destination + 4) = (int)*(uint*)(source + 4);
                    *(int*)(destination + 6) = (int)*(uint*)(source + 6);

                    destination += 8;
                    source += 8;

                    charsToCopy -= 8;
                }

                if ((charsToCopy & 4) != 0)
                {
                    *(int*)destination = (int)*(uint*)source;
                    *(int*)(destination + 2) = (int)*(uint*)(source + 2);

                    destination += 4;
                    source += 4;
                }

                if ((charsToCopy & 2) != 0)
                {
                    *(int*)destination = (int)*(uint*)source;

                    destination += 2;
                    source += 2;
                }

                if ((charsToCopy & 1) == 0)
                    return;

                *destination = *source;
            }
#endif

            public static unsafe int Strlen(byte* ptr)
            {
                var bytePtr = ptr;

                while ((int)*bytePtr != 0)
                    ++bytePtr;

                return (int)(bytePtr - ptr);
            }

            public static unsafe int Wcslen(char* ptr)
            {
                var chPtr = ptr;

                while (((int)(uint)chPtr & 3) != 0 && (int)*chPtr != 0)
                    ++chPtr;

                if ((int)*chPtr != 0)
                    while (((int)*chPtr & (int)chPtr[1]) != 0 || (int)*chPtr != 0 && (int)chPtr[1] != 0)
                        chPtr += 2;

                while ((int)*chPtr != 0)
                    ++chPtr;

                return (int)(chPtr - ptr);
            }
#endif
        }

        public static class HashCode
        {
#if !BRIDGE_NET
            public static unsafe int GetFrom(string value)
            {
                if (string.IsNullOrEmpty(value))
                    return 0;

                fixed (char* chPtr = value)
                    return GetFrom((byte*)chPtr, value.Length * 2);
            }

            public static unsafe int GetFrom(byte[] value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                fixed (byte* ptr = value)
                    return GetFrom(ptr, value.Length);
            }

            public static unsafe int GetFrom(byte[] value, int offset, int count)
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (offset < 0)
                    throw new ArgumentOutOfRangeException(nameof(offset));

                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));

                if (value.Length - offset < count)
                    throw new ArgumentException("Invalid value range");

                fixed (byte* ptr = value)
                    return GetFrom(ptr + offset, count);
            }

            public static unsafe int GetFrom(byte* ptr, int length)
            {
                var numPtr = (int*)ptr;

                var num1 = 352654597;
                var num2 = num1;
                length = length == 1 ? 1 : length / 2;

                while (length > 0)
                {
                    num1 = (num1 << 5) + num1 + (num1 >> 27) ^ *numPtr;

                    if (length <= 2)
                        break;

                    num2 = (num2 << 5) + num2 + (num2 >> 27) ^ numPtr[1];
                    numPtr += 2;
                    length -= 4;
                }

                return num1 + num2 * 1566083941;
            }

#else

            public static int GetFrom(string value)
            {
                if (string.IsNullOrEmpty(value))
                    return 0;

                var chars = value.ToCharArray();
                var bytes = new byte[chars.Length * 2];

                for (var i = 0; i < chars.Length; i++)
                {
                    bytes[i * 2] = (byte)chars[i];
                    bytes[i * 2 + 1] = (byte)(chars[i] >> 8);
                }

                return GetFrom(bytes, 0, bytes.Length);
            }

            public static int GetFrom(byte[] value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                return GetFrom(value, 0, value.Length);
            }

            public static int GetFrom(byte[] value, int offset, int count)
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (offset < 0)
                    throw new ArgumentOutOfRangeException(nameof(offset));

                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));

                if (value.Length - offset < count)
                    throw new ArgumentException("Invalid value range");

                var num1 = 352654597;
                var num2 = num1;
                var c = 0;
                var length = count == 1 ? 1 : count / 2;

                while (length > 0)
                {
                    num1 = (num1 << 5) + num1 + (num1 >> 27) ^ value[c];

                    if (length <= 2)
                        break;

                    num2 = (num2 << 5) + num2 + (num2 >> 27) ^ value[c + 1];
                    c += 2;
                    length -= 4;
                }

                return num1 + num2 * 1566083941;
            }
#endif
        }

        public static class ByteOrder
        {
#if !BRIDGE_NET

            public static unsafe int Swap(int value) => *(int*)Swap((byte*)&value, sizeof(int));
            public static unsafe uint Swap(uint value) => *(uint*)Swap((byte*)&value, sizeof(uint));
            public static unsafe long Swap(long value) => *(long*)Swap((byte*)&value, sizeof(long));
            public static unsafe short Swap(short value) => *(short*)Swap((byte*)&value, sizeof(short));
            public static unsafe float Swap(float value) => *(float*)Swap((byte*)&value, sizeof(float));
            public static unsafe ulong Swap(ulong value) => *(ulong*)Swap((byte*)&value, sizeof(ulong));
            public static unsafe double Swap(double value) => *(double*)Swap((byte*)&value, sizeof(double));
            public static unsafe ushort Swap(ushort value) => *(ushort*)Swap((byte*)&value, sizeof(ushort));
            public static unsafe decimal Swap(decimal value) => *(ushort*)Swap((byte*)&value, sizeof(ushort));

            static unsafe byte* Swap(byte* value, int size)
            {
                if (size == sizeof(ushort))
                {
                    *(ushort*)value = (ushort)((*(ushort*)value >> 8) | (*(ushort*)value << 8));
                }
                else if (size == sizeof(uint))
                {
                    var uint32 = *(uint*)value;
                    *(uint*)value = (uint32 >> 24) | ((uint32 & 0x00ff0000) >> 8) |
                                    ((uint32 & 0x0000ff00) << 8) | (uint32 << 24);
                }
                else if (size == sizeof(ulong))
                {
                    var uint64 = *(ulong*)value;
                    *(ulong*)value = (uint64 >> 56) | ((uint64 & 0x00ff000000000000L) >> 40) |
                                     ((uint64 & 0x0000ff0000000000L) >> 24) | ((uint64 & 0x000000ff00000000L) >> 8) |
                                     ((uint64 & 0x00000000ff000000L) << 8) | ((uint64 & 0x0000000000ff0000L) << 24) |
                                     ((uint64 & 0x000000000000ff00L) << 40) | (uint64 << 56);
                }
                else if (size == sizeof(decimal))
                {
                    for (var i = 0; i < sizeof(decimal); i++)
                    {
                        var tmp = value[sizeof(decimal) - 1 - i];
                        value[sizeof(decimal) - 1 - i] = value[i];
                        value[i] = tmp;
                    }
                }
                else
                    throw new ArgumentOutOfRangeException("Incorrect swap bytes order data size");

                return value;
            }
#else
            public static int Swap(int value) => value;
            public static uint Swap(uint value) => value;
            public static long Swap(long value) => value;
            public static short Swap(short value) => value;
            public static float Swap(float value) => value;
            public static ulong Swap(ulong value) => value;
            public static double Swap(double value) => value;
            public static ushort Swap(ushort value) => value;
            public static decimal Swap(decimal value) => value;
#endif
        }

        public static class Converter
        {
            //public static string FromSecureString(SecureString value)
            //{
            //    if (value?.Length == 0)
            //        return default(string);

            //    var result = (string)null;

            //    var charPtr = Marshal.SecureStringToCoTaskMemUnicode(value);
            //    unsafe { result = new string((char*)charPtr, 0, value.Length); }
            //    Marshal.ZeroFreeCoTaskMemUnicode(charPtr);

            //    return result;
            //}

            //public static SecureString ToSecureString(string value)
            //{
            //    if (string.IsNullOrEmpty(value))
            //        return null;

            //    var secureString = (SecureString)null;

            //    unsafe
            //    {
            //        fixed (char* charPtr = value)
            //            secureString = new SecureString(charPtr, value.Length);
            //    }

            //    secureString.MakeReadOnly();
            //    return secureString;
            //}

            public static byte[] FromHexString(string value)
            {
                var length = value.Length;
                var bytes = new byte[(length + 1) / 3];

                int CharConvert(int @char) => @char - (@char > 0x60 ? 0x57 : @char > 0x40 ? 0x37 : 0x30);

                for (int i = 0, j = 0; i < length; i += 3, ++j)
                    bytes[j] = (byte)((CharConvert(value[i]) << 4) + CharConvert(value[i + 1]));

                return bytes;
            }

            public static string ToHexString(byte[] value) => BitConverter.ToString(value);
        }

        public static class Reflection
        {
#if NET40 || NET35 || NET20 || BRIDGE_NET
            internal static BindingFlags GenericBindingFlags =
                BindingFlags.DeclaredOnly |
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.Public;
#endif

//            internal static System.Reflection.Assembly GetAssembly(Type type) =>
//#if NET40 || NET35 || NET20
//                type.Assembly;
//#else
//                type.GetTypeInfo().Assembly;
//#endif

            public static TAttribute GetCustomAssemblyAttribute<TAttribute>(Type type) where TAttribute : Attribute
//#if NET20
//            {
//                var attributes = type.Assembly.GetCustomAttributes(typeof(TAttribute), inherit: false);

//                if (attributes.Length > 0)
//                    return (TAttribute)attributes[0];

//                return default(TAttribute);
//            }
#if NET20 || NET40 || NET35 || BRIDGE_NET
            => (TAttribute)type.Assembly.GetCustomAttributes(typeof(TAttribute), inherit: false).FirstOrDefault();
#else
            => type.GetTypeInfo().Assembly.GetCustomAttribute<TAttribute>();
#endif

#if BRIDGE_NET

            public static bool IsPrimitive(Type type)
            {
                if (type == typeof(bool) ||
                    type == typeof(char) ||
                    type == typeof(byte) ||
                    type == typeof(sbyte) ||
                    type == typeof(short) ||
                    type == typeof(ushort) ||
                    type == typeof(int) ||
                    type == typeof(uint) ||
                    type == typeof(long) ||
                    type == typeof(ulong) ||
                    type == typeof(float) ||
                    type == typeof(double) ||
                    type == typeof(decimal))
                    return true;

                return false;
            }

#else

//            public static bool IsValueType(Type type) =>
//#if NET40 || NET35 || NET20
//                type.IsValueType;
//#else
//                type.GetTypeInfo().IsValueType;
//#endif

#endif

//            public static Type GetBaseType(Type type) =>
//#if NET40 || NET35 || NET20 || BRIDGE_NET
//                type.BaseType;
//#else
//                type.GetTypeInfo().BaseType;
//#endif

            public static FieldInfo GetDeclaredField(Type type, string name) =>
#if NET40 || NET35 || NET20 || BRIDGE_NET
                type.GetField(name, GenericBindingFlags);
#else
                type.GetTypeInfo().GetDeclaredField(name);
#endif

            public static IEnumerable<FieldInfo> GetDeclaredFields(Type type) =>
#if NET40 || NET35 || NET20 || BRIDGE_NET
                type.GetFields(GenericBindingFlags);
#else
                type.GetTypeInfo().DeclaredFields;
#endif

            public static IEnumerable<MethodInfo> GetDeclaredPublicMethods(Type type) =>
#if NET40 || NET35 || NET20 || BRIDGE_NET
                type.GetMethods(GenericBindingFlags);
#else
                type.GetTypeInfo().DeclaredMethods.Where(m => m.IsPublic);
#endif

            public static MethodInfo ConfigSetMethod(Type type, string propertyName) =>
#if BRIDGE_NET
                type.GetProperty(propertyName).SetMethod;
#elif NET40 || NET35 || NET20
                type.GetProperty(propertyName).GetSetMethod(nonPublic: false);
#else
                type.GetRuntimeProperty(propertyName).SetMethod;
#endif

//            public static bool IsAttributeDefined(MemberInfo memberInfo, Type attributeType) =>
//#if NET40 || NET35 || NET20
//                memberInfo.GetCustomAttributes(attributeType, inherit: false).Length != 0;
//#else
//                memberInfo.GetCustomAttribute(attributeType, inherit: false) != null;
//#endif

//            public static TAttribute GetCustomAttribute<TAttribute>(Type type) where TAttribute : Attribute
//#if NET20
//            {
//                var attributes = type.GetCustomAttributes(typeof(TAttribute), inherit: false);

//                if (attributes.Length > 0)
//                    return (TAttribute)attributes[0];

//                return default(TAttribute);
//            }
//#elif NET40 || NET35
//            => (TAttribute)type.GetCustomAttributes(typeof(TAttribute), inherit: false).FirstOrDefault();
//#else
//            => type.GetTypeInfo().GetCustomAttribute<TAttribute>(inherit: false);
//#endif

            public static Type[] GetGenericArguments(Type type) =>
#if NET40 || NET35 || NET20 || BRIDGE_NET
                type.GetGenericArguments();
#else
                type.GetTypeInfo().GenericTypeArguments;
#endif

            public static ConstructorInfo GetConstructor(Type type, Type[] parameters) =>
#if NET40 || NET35 || NET20 || BRIDGE_NET
                type.GetConstructor(parameters);
#else
                type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c =>
                    c.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters));
#endif

//            public static bool IsInterfaceImplemented(Type type, Type interfaceType)
//            {
//#if NET40 || NET35 || NET20
//                var interfaces = type.GetInterfaces();
//#else
//                var interfaces = type.GetTypeInfo().ImplementedInterfaces;
//#endif

//                foreach (var @interface in interfaces)
//                    if (@interface == interfaceType)
//                        return true;

//                return false;
//            }

            public static PropertyInfo GetAnyDeclaredProperty(Type type, string name) =>
#if NET40 || NET35 || NET20 || BRIDGE_NET
                type.GetProperty(name, GenericBindingFlags);
#else
                type.GetTypeInfo().DeclaredProperties.SingleOrDefault(p => p.Name == name);
#endif
        }

        public static class EncodedInteger
        {
            public const int OneByteCap = 128;
            public const int TwoBytesCap = 16384;
            public const int ThreeBytesCap = 2097152;
            public const int FourBytesCap = 268435456;

            public static int RequiredBytes(short value) => RequiredBytes((ulong)((value << 1) ^ (value >> 15)));
            //public static int RequiredBytes(int value) => RequiredBytes((ulong)((value << 1) ^ (value >> 31)));
            public static int RequiredBytes(int value) => RequiredBytes((uint)value);
            public static int RequiredBytes(long value) => RequiredBytes((ulong)((value << 1) ^ (value >> 63)));

            public static int RequiredBytes(ushort value) => RequiredBytes((ulong)value);
            public static int RequiredBytes(uint value) => RequiredBytes((ulong)value);

            public static int RequiredBytes(ulong value)
            {
                byte bytes = 0;

                while (value >= 0x80)
                {
                    bytes++;
                    value >>= 7;
                }

                return bytes + 1;
            }
        }

        internal static class ResourceStrings
        {
            internal const string

                ExceptionNegativeNumber = "Non-negative number required.",

                CyxorInternalException = "Cyxor internal exception.",

                ExceptionFormat = "Cyxor..{0}.{1}() : {2}",
                ExceptionFormat1 = "Cyxor..{0}.{1}({2}) : {3}",
                ExceptionFormat2 = "Cyxor..{0}.{1}({2}, {3}) : {4}",
                ExceptionFormat3 = "Cyxor..{0}.{1}({2}, {3}, {4}) : {5}",
                ExceptionFormat4 = "Cyxor..{0}.{1}({2}, {3}, {4}) : {5}",
                ExceptionMessageBufferDeserializeNumeric = "",
                ExceptionMessageBufferDeserializeObject = "Deserialization operation do not match format of bytes written in the Serialization process.";
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
