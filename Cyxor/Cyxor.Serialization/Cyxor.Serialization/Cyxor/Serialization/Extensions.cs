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
using System.Reflection;
using System.Collections.Generic;

#if !BRIDGE_NET
using System.IO;
#else
using System.Text;
#endif

//#if NET20
//namespace System.Runtime.CompilerServices
//{
//    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
//    public class ExtensionAttribute : Attribute { }
//}
//#else
//using System.Linq;
//#endif

namespace Cyxor.Extensions
{
    public static class StreamExtensions
    {
#if NET35
        public static void CopyTo(this System.IO.Stream input, System.IO.Stream output)
        {
            int bytesRead;
            var buffer = new byte[8192 * 4];

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, bytesRead);
        }
#endif

#if NET35 || NET40 || NETSTANDARD1_0
        public static bool TryGetBuffer(this MemoryStream value, out ArraySegment<byte> arraySegment)
        {
            var buffer = value.GetBuffer();
            arraySegment = new ArraySegment<byte>(buffer, 0, (int)value.Length);
            return true;
        }
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_3
        public static byte[] GetBuffer(this MemoryStream value)
#if NETSTANDARD1_0
            => value.ToArray();
#else
        {
            value.TryGetBuffer(out var arraySegment);
            return arraySegment.Array;
        }
#endif
#endif
    }

    public static class ReflectionExtensions
    {
#if NET20 || NET35 || NET40 || BRIDGE_NET
        public static Type GetTypeInfo(this Type type) => type;

        public static BindingFlags GenericBindingFlags =
            BindingFlags.DeclaredOnly |
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public |
            BindingFlags.NonPublic;

        public static BindingFlags GenericBindingFlagsPublic =
            BindingFlags.DeclaredOnly |
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public;

        public static BindingFlags GenericBindingFlagsNonPublic =
            BindingFlags.DeclaredOnly |
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.NonPublic;

        public static FieldInfo GetAnyDeclaredField(this Type type, string name)
            => type.GetField(name, GenericBindingFlags);

        public static IEnumerable<FieldInfo> GetDeclaredFields(this Type type)
            => type.GetFields(GenericBindingFlags);

        public static PropertyInfo GetAnyDeclaredProperty(this Type type, string name)
            => type.GetProperty(name, GenericBindingFlags);

        public static IEnumerable<PropertyInfo> GetDeclaredProperties(this Type type)
            => type.GetProperties(GenericBindingFlags);

        public static IEnumerable<PropertyInfo> GetDeclaredPublicProperties(this Type type)
            => type.GetProperties(GenericBindingFlagsPublic);

        public static MethodInfo GetAnyDeclaredMethod(this Type type, string name)
            => type.GetMethod(name, GenericBindingFlags);

        /// <summary>
        /// Returns an object that represents the specified public method declared by the current type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">The name of the method.</param>
        /// <returns>An object that represents the specified method, if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">name is null.</exception>
        public static MethodInfo GetDeclaredMethod(this Type type, string name) => type.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

        public static IEnumerable<MethodInfo> GetDeclaredMethods(this Type type, string name)
        {
            foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
                if (method.Name == name)
                    yield return method;
        }

        public static MethodInfo GetStaticMethod(this Type type, string name)
        {
            foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                if (method.Name == name)
                    return method;

            return null;
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this Type type, bool inherit = false)
            where TAttribute : Attribute
            => type.GetCustomAttributes(typeof(TAttribute), inherit).FirstOrDefault() as TAttribute;
        //{
        //    var attributes = type.GetCustomAttributes(typeof(TAttribute), inherit);

        //    if (attributes.Length > 0)
        //        return (TAttribute)attributes[0];

        //    return default(TAttribute);
        //}

        public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo element, bool inherit = false) where TAttribute : Attribute
        {
            var attributes = element.GetCustomAttributes(typeof(TAttribute), inherit);

            if (attributes.Length > 0)
                return (TAttribute)attributes[0];

            return default(TAttribute);
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this Type type, bool inherit = false) where TAttribute : Attribute
            => type.GetCustomAttributes(typeof(TAttribute), inherit) as IEnumerable<TAttribute>;

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MethodInfo methodInfo, bool inherit = false) where TAttribute : Attribute
            => methodInfo.GetCustomAttributes(typeof(TAttribute), inherit) as IEnumerable<TAttribute>;
#else
        public static MethodInfo GetStaticMethod(this Type type, string name)
            => type.GetTypeInfo().DeclaredMethods.Where(p => p.IsStatic && p.Name == name).SingleOrDefault();

        public static FieldInfo GetAnyDeclaredField(this Type type, string name)
            => type.GetTypeInfo().DeclaredFields.Where(p => p.Name == name).SingleOrDefault();

        public static IEnumerable<FieldInfo> GetDeclaredFields(this Type type)
            => type.GetTypeInfo().DeclaredFields;

        public static PropertyInfo GetAnyDeclaredProperty(this Type type, string name)
            => type.GetTypeInfo().DeclaredProperties.Where(p => p.Name == name).SingleOrDefault();

        public static IEnumerable<PropertyInfo> GetDeclaredProperties(this Type type)
            => type.GetTypeInfo().DeclaredProperties;

        public static IEnumerable<PropertyInfo> GetDeclaredPublicProperties(this Type type)
            => type.GetTypeInfo().DeclaredProperties.Where(p => p.GetMethod.IsPublic && p.SetMethod.IsPublic);

        public static MethodInfo GetAnyDeclaredMethod(this Type type, string name)
            => type.GetTypeInfo().DeclaredMethods.Where(p => p.Name == name).SingleOrDefault();
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_3 || BRIDGE_NET

        public static bool IsDefined(this Type type, Type attributeType) => IsDefined(type, attributeType, inherit: false);
        public static bool IsDefined(this Type type, Type attributeType, bool inherit) =>
#if !BRIDGE_NET
            type.GetTypeInfo().IsDefined(attributeType, inherit);
#else
            type.GetCustomAttributes(attributeType, inherit).Length > 0;
#endif

#if BRIDGE_NET
        public static bool IsDefined(this FieldInfo fieldInfo, Type attributeType)
            => IsDefined(fieldInfo, attributeType, inherit: false);

        public static bool IsDefined(this FieldInfo fieldInfo, Type attributeType, bool inherit)
            => fieldInfo.GetCustomAttributes(attributeType, inherit).Length > 0;

        public static bool IsDefined(this PropertyInfo propertyInfo, Type attributeType)
            => IsDefined(propertyInfo, attributeType, inherit: false);

        public static bool IsDefined(this PropertyInfo propertyInfo, Type attributeType, bool inherit)
            => propertyInfo.GetCustomAttributes(attributeType, inherit).Length > 0;
#endif

#endif

        public static bool IsInterfaceImplemented<T>(this Type type) => IsInterfaceImplemented(type, typeof(T));

        public static bool IsInterfaceImplemented(this Type type, Type interfaceType)
        {
#if NET20 || NET35 || NET40 || BRIDGE_NET
            var interfaces = type.GetInterfaces();
#else
            var interfaces = type.GetTypeInfo().ImplementedInterfaces;
#endif

            foreach (var @interface in interfaces)
                if (@interface == interfaceType)
                    return true;

            return false;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
