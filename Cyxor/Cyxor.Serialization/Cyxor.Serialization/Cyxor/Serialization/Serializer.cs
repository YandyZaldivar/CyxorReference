/*
  { Crasynet } - Core Asynchronous Networking <http://www.crasynet.com/>
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
using System.Text;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

#if !NET20
using System.Linq.Expressions;
#endif

#if !NET20 && !NET35 && !NET40
using System.Runtime.CompilerServices;
#endif

#if !NET20 && !NET35 && !NETSTANDARD1_0
using System.Collections.Concurrent;
#else
using System.Threading;
#endif

namespace Cyxor.Serialization
{
    using Extensions;

#if !BRIDGE_NET
    //using MethodDictionary = SortedDictionary<Type, MethodInfo>;
    using MethodDictionary = Dictionary<Type, MethodInfo>;
    using BufferOverflowException = EndOfStreamException;
#else
    using MethodDictionary = Dictionary<Type, MethodInfo>;
    using BufferOverflowException = InvalidOperationException;
#endif

    [DebuggerDisplay("{DebuggerDisplay()}")]
    public sealed class Serializer : Stream
    {
        #region Stream
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        public override long Position
        {
            get => position;
            set
            {
                if (value < 0 || value > length)
                    throw new ArgumentOutOfRangeException();

                position = (int)value;
            }
        }

        public override long Length => length;

        public override int ReadByte()
            => DeserializeByte();

        public override void WriteByte(byte value)
            => Serialize(value);

        public override int Read(byte[] buffer, int offset, int count)
        {
            DeserializeBytes(buffer, offset, count);
            return position == length ? 0 : count;
        }

        public bool StreamWriteRaw { get; set; }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!StreamWriteRaw)
                Serialize(buffer, offset, count);
            else
                SerializeRaw(buffer, offset, count);
        }

        public override void Flush() { }

        public override void SetLength(long value) => SetLength((int)value);

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: Position = offset; break;
                case SeekOrigin.End: Position = length - offset; break;
                case SeekOrigin.Current: Position = position + offset; break;
            }

            return position;
        }
        #endregion Stream

        sealed class Reflector
        {
            internal static ConcurrentCache<Type, TypeData> TypesCache = new ConcurrentCache<Type, TypeData>();
            internal static ConcurrentCache<Type, bool> KnownTypesCache = new ConcurrentCache<Type, bool>();

            internal sealed class TypeData : IEquatable<TypeData>
            {
                int HashCode;

                public TypeData Parent;
                public readonly Type Type;
                public readonly FieldData[] Fields;
                public readonly PropertyData[] Properties;

                public TypeData(Type type)
                {
                    Type = type;

                    #region Fields
                    var fields = type.GetDeclaredFields();

                    var fieldList = new List<FieldData>();

                    foreach (var field in fields)
                    {
                        var shouldSerialize = ShouldSerializeField(field);
                        //fieldList.Add(new FieldData(field, shouldSerialize));

                        // TODO: If the field is static the expressions will fail
                        if (shouldSerialize)
                            fieldList.Add(new FieldData(field, shouldSerialize));
                    }

                    Fields = fieldList.ToArray();
                    Array.Sort(Fields);
                    #endregion

                    #region Properties
                    var properties = type.GetDeclaredPublicProperties();

                    var propertyList = new List<PropertyData>();

                    foreach (var property in properties)
                    {
                        var shouldSerialize = ShouldSerializeProperty(property);

                        if (shouldSerialize)
                            propertyList.Add(new PropertyData(property, shouldSerialize));
                    }

                    Properties = propertyList.ToArray();
                    Array.Sort(Properties);
                    #endregion

                    HashCode = type.FullName.GetHashCode();
                }

                public override int GetHashCode() => HashCode;

                public bool Equals(TypeData other) => HashCode == other.HashCode;
            }

            internal sealed class FieldData : IComparable<FieldData>, IEquatable<FieldData>
            {
                readonly int HashCode;

                public readonly string Name;
                public readonly FieldInfo Field;
                public readonly bool ShouldSerialize;

                readonly Func<object, object> GetValueDelegate;
                readonly Action<object, object> SetValueDelegate;

                public override int GetHashCode() => HashCode;

                public object GetValue(object obj) => GetValueDelegate(obj);
                public void SetValue(object obj, object value) => SetValueDelegate(obj, value);

                public int CompareTo(FieldData other) => Name.CompareTo(other.Name);
                public bool Equals(FieldData other) => CompareTo(other) == 0;

#if !NET20
                private static void Map<T>(out T dest, T src) => dest = src;
                static MethodInfo MapMethodInfo = typeof(FieldData).GetStaticMethod(nameof(Map));
#endif

                public FieldData(FieldInfo field, bool shouldSerialize)
                {
                    Field = field;
                    Name = Field.Name;
                    ShouldSerialize = shouldSerialize;

                    HashCode = field.Name.GetHashCode();
#if NET20
                    GetValueDelegate = Field.GetValue;
                    SetValueDelegate = Field.SetValue;
#else
                    var mapMethodInfo = MapMethodInfo.MakeGenericMethod(Field.FieldType);

                    var valueParameter = Expression.Parameter(typeof(object), "value");
                    var objectParameter = Expression.Parameter(typeof(object), "object");

                    var valueConverted = Expression.Convert(valueParameter, Field.FieldType);
                    var objectConverted = Expression.Convert(objectParameter, Field.DeclaringType);

                    var fieldExpression = default(MemberExpression);

                    var memberExpression = ((Func<Expression, MemberExpression>)
                        (p => fieldExpression = Expression.Field(p, Field)))(objectConverted);

                    var getterExpression = Field.FieldType.GetTypeInfo().IsValueType ? 
                        Expression.Convert(memberExpression, typeof(object)) : (Expression)memberExpression;

                    var setterExpression = ((Func<Expression, Expression, MethodCallExpression>)
                        ((p, value) => Expression.Call(mapMethodInfo, fieldExpression, value)))(objectConverted, valueConverted);

                    GetValueDelegate = Expression.Lambda<Func<object, object>>(getterExpression, objectParameter).Compile();

                    SetValueDelegate = Expression.Lambda<Action<object, object>>(setterExpression, objectParameter, valueParameter).Compile();
#endif
                }
            }

            internal sealed class PropertyData : IComparable<PropertyData>, IEquatable<PropertyData>
            {
                readonly int HashCode;

                public readonly string Name;
                public readonly PropertyInfo Property;
                public readonly bool ShouldSerialize;

                readonly Func<object, object[], object> GetValueDelegate;
                readonly Action<object, object, object[]> SetValueDelegate;

                public override int GetHashCode() => HashCode;

                public object GetValue(object obj, object[] index) => GetValueDelegate(obj, index);
                public void SetValue(object obj, object value, object[] index) => SetValueDelegate(obj, value, index);

                public int CompareTo(PropertyData other) => Name.CompareTo(other.Name);
                public bool Equals(PropertyData other) => CompareTo(other) == 0;

#if !NET20
                private static void Map<T>(out T dest, T src) => dest = src;
                static MethodInfo MapMethodInfo = typeof(PropertyData).GetStaticMethod(nameof(Map));
#endif

                public PropertyData(PropertyInfo property, bool shouldSerialize)
                {
                    Property = property;
                    Name = Property.Name;
                    ShouldSerialize = shouldSerialize;

                    HashCode = property.Name.GetHashCode();
#if NET20
                    GetValueDelegate = Property.GetValue;
                    SetValueDelegate = Property.SetValue;
#else
                    var mapMethodInfo = MapMethodInfo.MakeGenericMethod(Property.PropertyType);

                    var valueParameter = Expression.Parameter(typeof(object), "value");
                    var indexParameter = Expression.Parameter(typeof(object[]), "index");
                    var objectParameter = Expression.Parameter(typeof(object), "object");

                    var valueConverted = Expression.Convert(valueParameter, Property.PropertyType);
                    //var indexConverted = Expression.Convert(valueParameter, typeof(object[]));
                    var objectConverted = Expression.Convert(objectParameter, Property.DeclaringType);

                    var propertyExpression = default(MemberExpression);

                    var memberExpression = ((Func<Expression, MemberExpression>)
                        (p => propertyExpression = Expression.Property(p, Property)))(objectConverted);

                    var getterExpression = Property.PropertyType.GetTypeInfo().IsValueType ? 
                        Expression.Convert(memberExpression, typeof(object)) : (Expression)memberExpression;

                    var setterExpression = ((Func<Expression, Expression, MethodCallExpression>)
                        ((p, value) => Expression.Call(mapMethodInfo, propertyExpression, value)))(objectConverted, valueConverted);

                    GetValueDelegate = Expression.Lambda<Func<object, object[], object>>(getterExpression, objectParameter, indexParameter).Compile();

                    SetValueDelegate = Expression.Lambda<Action<object, object, object[]>>(setterExpression, objectParameter, valueParameter, indexParameter).Compile();
#endif
                }
            }

            internal class ConcurrentCache<TKey, TValue>
            {
#if NET20 || NET35 || NETSTANDARD1_0

#if !NET20
                ReaderWriterLockSlim RwLock = new ReaderWriterLockSlim();
#else
                ReaderWriterLock RwLock = new ReaderWriterLock();
#endif
                readonly Dictionary<TKey, TValue> Items = new Dictionary<TKey, TValue>();

                public bool TryAdd(TKey key, TValue value)
                {
#if NET20
                    RwLock.AcquireWriterLock(Timeout.Infinite);
#else
                    RwLock.EnterWriteLock();
#endif
                    try
                    {
                        Items.Add(key, value);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
#if NET20
                        RwLock.ReleaseWriterLock();
#else
                        RwLock.ExitWriteLock();
#endif
                    }
                }

                public bool TryGetValue(TKey key, out TValue value)
                {
#if NET20
                    RwLock.AcquireReaderLock(Timeout.Infinite);
#else
                    RwLock.EnterReadLock();
#endif
                    try
                    {
                        return Items.TryGetValue(key, out value);
                    }
                    finally
                    {
#if NET20
                        RwLock.ReleaseReaderLock();
#else
                        RwLock.ExitReadLock();
#endif
                    }
                }
#else
                readonly ConcurrentDictionary<TKey, TValue> Items = new ConcurrentDictionary<TKey, TValue>();

                public bool TryAdd(TKey key, TValue value) => Items.TryAdd(key, value);

                public bool TryGetValue(TKey key, out TValue value) => Items.TryGetValue(key, out value);
#endif
            }

            internal static class Delegate
            {
                #region Func
                static MethodInfo CreateFuncMethodInfo = typeof(Delegate).GetStaticMethod(nameof(CreateFunc));
                static ConcurrentCache<Type, Func<Serializer, object>> FuncDelegateCache = new ConcurrentCache<Type, Func<Serializer, object>>();

                public static Func<Serializer, object> GetFunc(Type type)
                {
                    if (!FuncDelegateCache.TryGetValue(type, out var func))
                        FuncDelegateCache.TryAdd(type, func = GetFunc<Serializer>(GetSerializationMethod(type, SerializerOperation.Deserialize)));

                    return func;
                }

                static Func<T, object> GetFunc<T>(MethodInfo methodInfo) where T : class
                    => (Func<T, object>)CreateFuncMethodInfo.MakeGenericMethod(typeof(T), methodInfo.ReturnType).Invoke(null, new object[] { methodInfo });

                static Func<TTarget, object> CreateFunc<TTarget, TReturn>(MethodInfo method) where TTarget : class
                {
#if NET20 || NET35 || NET40
                    var func = (Func<TTarget, TReturn>)System.Delegate.CreateDelegate(typeof(Func<TTarget, TReturn>), method);
#else
                    var func = (Func<TTarget, TReturn>)method.CreateDelegate(typeof(Func<TTarget, TReturn>));
#endif
                    return (target) => func(target);
                }
                #endregion

                #region Action
                static MethodInfo CreateActionMethodInfo = typeof(Delegate).GetStaticMethod(nameof(CreateAction));
                static ConcurrentCache<Type, Action<Serializer, object>> ActionDelegateCache = new ConcurrentCache<Type, Action<Serializer, object>>();

                public static Action<Serializer, object> GetAction(Type type)
                {
                    if (!ActionDelegateCache.TryGetValue(type, out var action))
                        ActionDelegateCache.TryAdd(type, action = GetAction<Serializer>(GetSerializationMethod(type, SerializerOperation.Serialize)));

                    return action;
                }

                static Action<T, object> GetAction<T>(MethodInfo methodInfo) where T : class
                    => (Action<T, object>)CreateActionMethodInfo.MakeGenericMethod
                        (typeof(T), methodInfo.GetParameters().FirstOrDefault().ParameterType)
                        .Invoke(null, new object[] { methodInfo });

                static Action<TTarget, object> CreateAction<TTarget, TParam>(MethodInfo method) where TTarget : class
                {
#if NET20 || NET35 || NET40
                    var action = (Action<TTarget, TParam>)System.Delegate.CreateDelegate(typeof(Action<TTarget, TParam>), method);
#else
                    var action = (Action<TTarget, TParam>)method.CreateDelegate(typeof(Action<TTarget, TParam>));
#endif
                    return (target, param) => action(target, (TParam)param);
                }
                #endregion
            }

            static bool ShouldSerializeField(FieldInfo field)
            {
                if (field.IsDefined(typeof(CyxorIgnoreAttribute), inherit: false))
                    return false;

                if (field.IsStatic && field.IsInitOnly)
                    return false;

                if (field.Name[0] != '<')
                     return true;

                var propertyName = field.Name.Substring(1, field.Name.IndexOf('>') - 1);
                var property = field.DeclaringType.GetAnyDeclaredProperty(propertyName);

                return ShouldSerializeProperty(property);
            }

            static bool ShouldSerializeProperty(PropertyInfo property)
            {
                if (property.IsDefined(typeof(CyxorIgnoreAttribute), inherit: false))
                    return false;

                return true;
            }

            public static bool IsKnownType(Type type)
            {
                //if (!KnownTypes.TryGetValue(type, out var result))
                if (!KnownTypesCache.TryGetValue(type, out var result))
                {
                    result = type.IsArray ||
                                 type == typeof(Uri) ||
                                 type == typeof(Guid) ||
                                 type == typeof(string) ||
                                 type == typeof(decimal) ||
                                 type == typeof(TimeSpan) ||
                                 type == typeof(DateTime) ||
                                 type == typeof(Serializer) ||
                                 type == typeof(BitSerializer) ||
                                 type.GetTypeInfo().IsEnum ||

#if BRIDGE_NET
                         Utilities.Reflection.IsPrimitive(type) ||
#else
                         type.GetTypeInfo().IsPrimitive ||
                                 type == typeof(DateTimeOffset) ||
#endif

                                 type.IsInterfaceImplemented(typeof(IEnumerable));
                    //type == typeof(IEnumerable<>) ||
                    //type is IEnumerable;



                    //type.IsInterfaceImplemented(typeof(IEnumerable));
                    //Utilities.Reflection.IsInterfaceImplemented(type, nameof(IEnumerable));// ||
                    //Utilities.Reflection.IsInterfaceImplemented(type, nameof(ISerializable));

                    KnownTypesCache.TryAdd(type, result);

//#if NET20 || NET35 || NETSTANDARD1_0
//                    KnownTypes.Add(type, result);
//#else
//                    KnownTypes.TryAdd(type, result);
//#endif
                }

                return result;
            }
        }

        bool AutoRaw;

        //public const int CompressedIntThreshold = 20;
        public const int CompressedIntThreshold = 0;

        #region Static

        static Type NullableGenericType;


        static MethodDictionary SerializeMethods;
        static MethodDictionary DeserializeMethods;

        static Serializer()
        {
            SerializeMethods = GetSerializeMethods();
            DeserializeMethods = GetDeserializeMethods();

            ByteOrder = BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

            foreach (var deserializeMethod in DeserializeMethods)
                if (!deserializeMethod.Value.IsGenericMethodDefinition)
                    Reflector.Delegate.GetFunc(deserializeMethod.Key);

            foreach (var serializeMethod in SerializeMethods)
                if (!serializeMethod.Value.IsGenericMethodDefinition)
                    Reflector.Delegate.GetAction(serializeMethod.Key);
        }

        /// <summary>
        /// This is necessary for collections other than List<>
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        static object CheckFieldCollectionType(object fieldValue, Type fieldType)
        {
            // TODO: Review, could be an object with an IEnumerable<> MyObject

            if (fieldValue != null)
                if (fieldType.GetTypeInfo().IsGenericType)
                    if (!fieldType.GetTypeInfo().IsInterface)
                        if (fieldType.IsInterfaceImplemented(typeof(IEnumerable)))
                            fieldValue = Activator.CreateInstance(fieldType, fieldValue);

            return fieldValue;
        }

        static MethodDictionary GetDeserializeMethods()
        {
            var methods = Utilities.Reflection.GetDeclaredPublicMethods(typeof(Serializer));

            var operationName = nameof(SerializerOperation.Deserialize);
            var deserializeMethods = new MethodDictionary();
            //var deserializeMethods = new MethodDictionary(TypeComparer.Instance);

            foreach (var type in SerializeMethods.Keys)
            {
                var serializeTypeName = type.Name;

                if (serializeTypeName.StartsWith(nameof(Nullable)))
                    serializeTypeName = nameof(Nullable) + Utilities.Reflection.GetGenericArguments(type)[0].Name;

                foreach (var method in methods)
                    if (method.GetParameters().Length == 0 && method.Name.StartsWith(operationName))
                    {
                        var typeName = method.Name.Substring(operationName.Length);

                        if (string.IsNullOrEmpty(typeName))
                            continue;

                        if (serializeTypeName != typeName)
                            if (serializeTypeName != typeName.Substring(0, typeName.Length - 1) + "[]")
                                if (!(serializeTypeName == nameof(IDictionary) && typeName == nameof(IEnumerable)))
                                    continue;
                                else if (method.GetGenericArguments().Length != 2)
                                    continue;

                        if (serializeTypeName == nameof(IEnumerable))
                            if (method.GetGenericArguments().Length != 1)
                                continue;

                        if (method.Name == nameof(DeserializeNullableT))
                            NullableGenericType = type;

                        deserializeMethods[type] = method;
                        break;
                    }
            }

            if (deserializeMethods.Count != SerializeMethods.Count)
                throw new Exception("Errors initializing Cyxor serialization.");

            return deserializeMethods;
        }

        static MethodDictionary GetSerializeMethods()
        {
            var methods = Utilities.Reflection.GetDeclaredPublicMethods(typeof(Serializer));

            var operationName = nameof(SerializerOperation.Serialize);
            var serializeMethods = new MethodDictionary();
            //var serializeMethods = new MethodDictionary(TypeComparer.Instance);

            foreach (var method in methods)
                if (method.Name == operationName)
                {
                    var parameters = method.GetParameters();

                    if (parameters.Length == 1)
                    {
                        if (!parameters[0].ParameterType.Name.EndsWith("*"))
                        {
                            var keyType = parameters[0].ParameterType;

                            if (keyType.IsArray)
                            {
                                if (keyType.GetElementType().IsGenericParameter)
                                    keyType = typeof(Array);
                            }
                            else if (keyType.GetTypeInfo().IsGenericType)
                            {
                                if (keyType.Name.StartsWith(nameof(IEnumerable)))
                                {
                                    if (method.GetGenericArguments().Length == 1)
                                        keyType = typeof(IEnumerable);
                                    else if (method.GetGenericArguments().Length == 2)
                                        keyType = typeof(IDictionary);
                                }
                            }
                            else if (keyType.IsGenericParameter && keyType.Name == "T")
                                keyType = typeof(object);

                            serializeMethods[keyType] = method;
                        }
                    }
                }

            return serializeMethods;
        }

        //sealed class FieldInfoComparer : IComparer<FieldInfo>
        //{
        //    public static FieldInfoComparer Instance = new FieldInfoComparer();
        //    public int Compare(FieldInfo x, FieldInfo y) => x.Name.CompareTo(y.Name);
        //}

        public static Serializer XSerialize(object value)
        {
            var serializer = new Serializer();
            serializer.SerializeRaw(value);
            return serializer;
        }

        public static T XDeserialize<T>(byte[] buffer) => new Serializer(buffer).ToObject<T>();
        public static T XDeserialize<T>(ArraySegment<byte> buffer) => new Serializer(buffer).ToObject<T>();
        public static T XDeserialize<T>(byte[] buffer, int offset, int count) => new Serializer(buffer, offset, count).ToObject<T>();

        public static object XDeserialize(byte[] buffer, Type type) => new Serializer(buffer).ToObject(type);
        public static object XDeserialize(ArraySegment<byte> buffer, Type type) => new Serializer(buffer).ToObject(type);
        public static object XDeserialize(byte[] buffer, int offset, int count, Type type) => new Serializer(buffer, offset, count).ToObject(type);

#endregion Static

#region Reflector

        internal enum SerializerOperation
        {
            Serialize,
            Deserialize,
        }

        class ObjectSerialization
        {
            readonly bool Raw;

            internal int BufferPosition;
            internal int PositionLenght;

            internal ObjectSerialization Next;
            internal ObjectSerialization Previous;

            internal ObjectSerialization(int bufferPosition, bool raw, ObjectSerialization previous)
            {
                Raw = raw;
                PositionLenght = 1;
                BufferPosition = bufferPosition;

                if (previous != null)
                {
                    Previous = previous;
                    previous.Next = this;
                }
            }

            void AdjustBufferPosition(int offset)
            {
                BufferPosition += offset;
                Next?.AdjustBufferPosition(offset);
            }

            internal void Step(Serializer serializer, int size)
            {
                var count = (serializer.position + size) - BufferPosition;
                 
                if (count < Utilities.EncodedInteger.OneByteCap || Raw)
                    return;

                var positionLenght = count < Utilities.EncodedInteger.TwoBytesCap ? 2 :
                    count < Utilities.EncodedInteger.ThreeBytesCap ? 3 :
                    count < Utilities.EncodedInteger.FourBytesCap ? 4 : 5;

                if (PositionLenght < positionLenght)
                    AdjustBuffer();

                void AdjustBuffer()
                {
                    var differenceBytes = positionLenght - PositionLenght;

                    serializer.ObjectSerializationActive = !serializer.ObjectSerializationActive;
                    serializer.EnsureCapacity(differenceBytes + size, SerializerOperation.Serialize);
                    serializer.ObjectSerializationActive = !serializer.ObjectSerializationActive;

#if !BRIDGE_NET
                    System.Buffer.BlockCopy(
#else
                    Utilities.Memory.BlockCopy(
#endif
                    serializer.buffer, BufferPosition, serializer.buffer, BufferPosition + differenceBytes, serializer.position - BufferPosition);

                    PositionLenght += differenceBytes;
                    serializer.position += differenceBytes;
                    Next?.AdjustBufferPosition(differenceBytes);
                }
            }
        }

        //static readonly int SerializationListBaseCapacity = 16;

        int CircularReferencesIndex;
        readonly bool CircularReferencesActive = true;
        Dictionary<object, int> CircularReferencesDictionary = new Dictionary<object, int>();
        Dictionary<int, object> CircularReferencesIndexDictionary = new Dictionary<int, object>();

        readonly bool UseObjectSerialization = true;
        bool ObjectSerializationActive;
        Stack<ObjectSerialization> SerializationStack = new Stack<ObjectSerialization>();

        public static string GenerateSerializationSchema()
        {
            var result = new StringBuilder();

            foreach (var method in SerializeMethods)
            {
                result.AppendLine(method.Value.ToString());

                if (DeserializeMethods.ContainsKey(method.Key))
                    result.AppendLine($"'-->{DeserializeMethods[method.Key]}{Environment.NewLine}");
                else
                    result.AppendLine("Failed");
            }

            return result.ToString();
        }

        static MethodInfo GetSerializationMethod(Type type, SerializerOperation operation)
        {
            var dictionary = operation == SerializerOperation.Serialize ? SerializeMethods : DeserializeMethods;

#if !BRIDGE_NET
            if (type.IsByRef)
                type = type.GetElementType();
#endif

            var suitableType = type;
            var genericArgumentsType = default(Type[]);

            if (type.IsInterfaceImplemented(typeof(ISerializable)))
            {
                suitableType = typeof(ISerializable);
                genericArgumentsType = new Type[] { type };
            }
            else if (type.GetTypeInfo().IsEnum)
            {
                suitableType = typeof(Enum);
                genericArgumentsType = new Type[] { type };
            }
            else if (type.IsArray && type.GetElementType() != typeof(byte) && type.GetElementType() != typeof(char))
            {
                suitableType = typeof(Array);
                genericArgumentsType = new Type[] { type.GetElementType() };
            }
            else if (type != typeof(string) && type.IsInterfaceImplemented(typeof(IEnumerable)))
            {
                if (type.GetTypeInfo().IsGenericType)
                {
                    genericArgumentsType = Utilities.Reflection.GetGenericArguments(type);

                    if (genericArgumentsType.Length == 1)
                        suitableType = typeof(IEnumerable);
                    else if (genericArgumentsType.Length == 2)
                        suitableType = typeof(IDictionary);
                }
            }

            if (!dictionary.TryGetValue(suitableType, out var method))
            {
                if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    dictionary.TryGetValue(NullableGenericType, out method);
                else
                {
                    method = dictionary[typeof(object)];
                    genericArgumentsType = new Type[] { type };
                }
            }

            // TODO: method.MakeGenericMethod not supported by Bridge
#if !BRIDGE_NET
            if (method.IsGenericMethodDefinition)
                method = method.MakeGenericMethod(genericArgumentsType ?? Utilities.Reflection.GetGenericArguments(type));
#endif

            return method;
        }

#endregion Reflector

#region Core

        public Serializer() { }

        public Serializer(byte[] value) => SetBuffer(value);

        public Serializer(string value) => SerializeRaw(value);

        public Serializer(object value) => SerializeRaw(value);

        public Serializer(Serializer value) => SerializeRaw(value);

        public Serializer(ArraySegment<byte> arraySegment) => SetBuffer(arraySegment);

        public Serializer(byte[] buffer, int offset, int count) => SetBuffer(buffer, offset, count);

        public Serializer(object value, IBackingSerializer backingSerializer) => SerializeRaw(value, backingSerializer);

#region Public properties

        /// <summary>
        /// Represents the empty Serializer. This field is readonly.
        /// </summary>
        public static Serializer Empty = new Serializer();

        /// <summary>
        /// Represents the Serializer character encoding. This field is readonly.
        /// </summary>
        public static UTF8Encoding Encoding = new UTF8Encoding(false, true);


        /// <summary>
        /// Indicates the Serializer default byte order ("endianess").
        /// All values written to serializer instances will be converted to the specified byte order if necessary and
        /// values read will be converted to this computer architecture byte order if necessary. The initial value
        /// is always equal to the current computer architecture and by default swapping never occurs.
        /// </summary>
        /// <value>
        /// The byte order.
        /// </value>
        public static ByteOrder ByteOrder;

        /// <summary>
        /// The maximum number of bytes this buffer can hold.
        /// </summary>
        /// <remarks>
        /// This value is never bigger than the maximum value of a signed 32-bit integer.
        /// </remarks>
        /// <returns>
        /// The maximum number of bytes this buffer can hold.
        /// </returns>
        public const int MaxCapacity = int.MaxValue - 64;

        public object Tag;

        public bool ReadOnly;

        int position;
        /// <summary>
        /// Gets the current position within the buffer.
        /// </summary>
        /// <returns>
        /// The current position within the buffer.
        /// </returns>
        public int Int32Position
        {
            get => position;
            set
            {
                if (value < 0 || value > length)
                    throw new ArgumentOutOfRangeException();

                position = value;
            }
        }

        /// <summary>
        /// Sets the current position within the serializer buffer.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="value">
        /// The value at which to set the position.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The position is set to a negative value or a value greater than the current buffer Length.
        /// </exception>
        //public void SetPosition(int value)
        //{
        //    if (value < 0 || value > Length)
        //        throw new ArgumentOutOfRangeException();

        //    Position = value;
        //}

        public int Count => length - position;

        int length;

        /// <summary>
        /// Gets and sets the length of the buffer to the specified value.
        /// </summary>
        public int Int32Length => length;

        /// <remarks>
        /// If the value is bigger than the actual Length the buffer will try to expand. The Position within
        /// the buffer remains unchangeable, but the <see cref="Capacity"/> increments if necessary.
        ///
        /// If the value is smaller than the actual Length the Length is updated with the new value. If the
        /// original Position is smaller than the new Length value it remains unchangeable, otherwise it is set
        /// to the value of Length.
        /// </remarks>
        /// <returns>
        /// The length of the buffer in bytes.
        /// </returns>
        /// <param name="value">
        /// The value at which to set the length.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is negative or is greater than the maximum capacity of the Buffer,
        /// where the maximum length is <see cref="MaxCapacity"/>.
        /// </exception>
        public void SetLength(int value)
        {
            if (value < 0 || value > MaxCapacity)
                throw new ArgumentOutOfRangeException();

            if (length == value)
                return;

            var startPosition = position;

            if (value > Capacity)
            {
                position = 0;
                EnsureCapacity(value, SerializerOperation.Serialize);
            }
            else
                length = value;

            if (startPosition < length)
                position = startPosition;
            else
                position = length;
        }

        /// <summary>
        /// Gets the number of bytes allocated for this serializer.
        /// </summary>
        /// <returns>
        /// The length of the usable portion of the serializer buffer.
        /// </returns>
        public int Capacity;

        public int GetCapacity() => Capacity;

        byte[] buffer;
        /// <summary>
        /// Gets the underlying array of unsigned bytes of this serializer.
        /// </summary>
        /// <returns>
        /// The underlying array of unsigned bytes of this serializer.
        /// </returns>
        public byte[] Buffer
        {
            get => buffer;
            private set
            {
                if (buffer != null)
                    MemoryDisposed?.Invoke(this, EventArgs.Empty);

                buffer = value;
            }
        }

#endregion

        public event EventHandler MemoryDisposed;

#region Public

        public static bool IsNullOrEmpty(Serializer serializer)
        {
            if (serializer == null)
                return true;

            if (serializer == Serializer.Empty)
                return true;

            if (serializer.length == 0)
                return true;

            return false;
        }

        public string ToHexString()
        {
            var hex = buffer != null ? BitConverter.ToString(buffer, 0, length) : "null";
            return $"[{position}-{length}-{Capacity}]  {hex}";
        }

        string DebuggerDisplay()
        {
            var count = length > 20 ? 20 : length;

            var sb = new StringBuilder(count * 2 + 3);

            if (length == 0)
                sb.Append(buffer == null ? "null" : $"byte[{buffer.Length}]");
            else
                for (var i = 0; i < count; sb.Append('-'))
                    sb.Append(buffer[i++]);

            if (length > count)
                sb.Insert(sb.Length - 1, "...");

            if (length != 0)
                sb.Remove(sb.Length - 1, 1);

            return $"[{nameof(position)} = {position}]  [{nameof(length)} = {length}]  [{nameof(Capacity)} = {Capacity}]  [Data = {sb}]";
        }

        public void SetBuffer(byte[] buffer) => SetBuffer(buffer, 0, (int)buffer?.Length);

        public void SetBuffer(ArraySegment<byte> arraySegment)
            => SetBuffer(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

        public void SetBuffer(byte[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (index < 0 || index > count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Buffer = buffer;
            length = count;
            position = index;
            Capacity = buffer.Length;
        }

        /// <summary>
        /// Sets the length of the current buffer to the specified value.
        /// </summary>
        /// <remarks>
        /// This method behaves similar to SetLength() except that it doesn't modified the current <see cref="length"/>
        /// if the <paramref name="value"/> is not smaller than current <see cref="Capacity"/>.
        /// </remarks>
        /// <param name="value">
        /// The value at which to set the length.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="value"/> is negative or is greater than the maximum capacity of the Buffer,
        /// where the maximum capacity is <see cref="MaxCapacity"/>.
        /// </exception>
        public void SetCapacity(int value)
        {
            if (value < 0 || value > MaxCapacity)
                throw new ArgumentOutOfRangeException();

            if (Capacity == value)
                return;

            var startLength = length;
            var startPosition = position;

            if (value > Capacity)
            {
                position = 0;
                EnsureCapacity(value, SerializerOperation.Serialize);
                length = startLength;
            }
            else
            {
                Buffer = null;
                Capacity = length = position = 0;
                EnsureCapacity(value, SerializerOperation.Serialize);

                if (length > startLength)
                    length = startLength;
            }

            if (startPosition < length)
                position = startPosition;
            else
                position = length;
        }

        /// <summary>
        /// Quick reset of the buffer content by setting the <see cref="length"/> and <see cref="Int32Position"/> to 0.
        /// The actual allocated <see cref="Capacity"/> remains intact.
        /// </summary>
        public void Reset() => Reset(0);

        /// <summary>
        /// Resets the contents of the buffer.
        /// </summary>
        /// <remarks>
        /// The parameter <paramref name="numBytes"/> suggest when to perform a full reset or not. If the actual
        /// <see cref="Capacity"/> is greater than <paramref name="numBytes"/> the buffer is fully reseted and
        /// <see cref="Capacity"/> is set to zero. If the <see cref="Capacity"/> is lower than <paramref name="numBytes"/>
        /// or <paramref name="numBytes"/> is equal to zero this method behaves the same as Reset().
        /// For setting the buffer <see cref="Capacity"/> to an exact value use SetCapacity().
        /// </remarks>
        /// <param name="numBytes">
        /// The maximum number of bytes allowed to remain allocated without performing a full reset.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="numBytes"/> is negative or is greater than the maximum capacity of the Buffer,
        /// where the maximum capacity is <see cref="MaxCapacity"/>.
        /// </exception>
        public void Reset(int numBytes)
        {
            if (numBytes < 0 || numBytes > MaxCapacity)
                throw new ArgumentOutOfRangeException(nameof(numBytes));

            if (Capacity <= numBytes || numBytes == 0)
                length = position = 0;
            else
            {
                Capacity = length = position = 0;
                Buffer = null;
            }
        }

        public void Insert(Serializer value)
        {
            if (value == null)
                throw new ArgumentNullException();
            else if (value.length == 0)
                return;

            EnsureCapacity(value.length, SerializerOperation.Serialize);

            var auxBuffer = new Serializer();
            auxBuffer.SerializeRaw(buffer, position, length - position);
            SerializeRaw(value);
            SerializeRaw(auxBuffer);
        }

        public void Delete() => Delete(0, 0);

        public void Delete(int index) => Delete(index, 0);

        // TODO: Check this method
        public void Delete(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), Utilities.ResourceStrings.ExceptionNegativeNumber);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), Utilities.ResourceStrings.ExceptionNegativeNumber);

            if (length - index < count)
                throw new ArgumentException();

            //if (index > Length)
            //    throw new IndexOutOfRangeException();

            //if (count == 0 && index != 0)
            //    throw new ArgumentException();

            //if ((uint)(index + count) > Length)
            //    throw new IndexOutOfRangeException();

            if (index == 0 && count == 0)
            {
                Capacity = length = position = 0;
                Buffer = null;
            }
            else if (index == 0 && count == length)
            {
                length = position = 0;
            }
            else
            {
#if !BRIDGE_NET
                System.Buffer.BlockCopy(
#else
                Utilities.Memory.BlockCopy(
#endif
                    buffer, index + count, buffer, index, length - count);

                position = index;
                length = length - count;
            }
        }

        public void Pop(int count) => Delete(0, count);

        public void PopChars(int count)
        {
            if (count == 0)
                throw new ArgumentException();

            if (count < 0 || count > length)
                throw new ArgumentOutOfRangeException();

            position = 0;

            for (var i = 0; i < count; i++)
                DeserializeChar();

            count = position;

#if !BRIDGE_NET
            System.Buffer.BlockCopy(
#else
            Utilities.Memory.BlockCopy(
#endif
                buffer, count, buffer, 0, length -= count);

            position = 0;
        }

        /// <summary>
        /// Returns the hash code for this buffer.
        /// </summary>
        /// <remarks>
        /// This method uses all the bytes in the buffer to generate a reasonably randomly distributed output
        /// that is suitable for use in hashing algorithms and data structures such as a hash table.
        /// </remarks>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => Utilities.HashCode.GetFrom(buffer, 0, length);

        public object Clone()
        {
            var value = new Serializer();
            value.SerializeRaw(this);

            value.length = length;
            value.position = position;

            return value;
        }

#endregion Public

#region Private

        public void EnsureCapacity(int size) => EnsureCapacity(size, SerializerOperation.Serialize);

        void EnsureCapacity(int size, SerializerOperation operation)
        {
            if (size < 0)
                throw new ArgumentException("size", "Ensure capacity <negative size>");

            if (operation != SerializerOperation.Deserialize && ReadOnly)
                throw new InvalidOperationException("The buffer is marked as ReadOnly.");

            if ((uint)(position + size) > length)
            {
                if (operation == SerializerOperation.Deserialize)
                    throw new BufferOverflowException();
                else
                {
                    if (position + size < 0)
                        throw new BufferOverflowException();
                    else if (position + size <= Capacity)
                        length = position + size;
                    else
                    {
                        if (buffer == null)
                        {
                            Capacity = length = size;

                            if (Capacity < 256)
                                Capacity = 256;

                            buffer = new byte[Capacity];
                        }
                        else if (position + size > Capacity)
                        {
                            if (position + size > (uint)(Capacity + Capacity / 2))
                                Capacity = position + size;
                            else if ((uint)(Capacity + Capacity / 2) >= MaxCapacity)
                                Capacity = MaxCapacity;
                            else
                                Capacity += Capacity / 2;

                            var newBuffer = new byte[Capacity];

#if !BRIDGE_NET
                            System.Buffer.BlockCopy(
#else
                            Utilities.Memory.BlockCopy(
#endif
                                src: buffer, srcOffset: 0, dst: newBuffer, dstOffset: 0, count: length);

                            length = position + size;
                            Buffer = newBuffer;
                        }
                    }
                }
            }

            if (UseObjectSerialization)
            if (ObjectSerializationActive && operation == SerializerOperation.Serialize)
            {
                //if (SerializationStack.Count == 0)
                //    throw new Exception(Utilities.ResourceStrings.CyxorInternalException);

                //foreach (var objectSerialization in SerializationStack)
                //    objectSerialization.Step(this, size);

                var objectSerialization = SerializationStack.Peek();

                while (objectSerialization != null)
                {
                    objectSerialization.Step(this, size);
                    objectSerialization = objectSerialization.Previous;
                }
            }
        }

#endregion Private

#endregion Core

#region ToConversions

#if !BRIDGE_NET
#if NETSTANDARD1_0
        public MemoryStream ToMemoryStream() => new MemoryStream(Buffer, 0, length, writable: false);
#else
        public MemoryStream ToMemoryStream() => new MemoryStream(buffer, 0, length, writable: false, publiclyVisible: false);
#endif
#endif

        public byte[] ToByteArray()
        {
            var value = new byte[length];

#if !BRIDGE_NET
            System.Buffer.BlockCopy(
#else
            Utilities.Memory.BlockCopy(
#endif
                src: buffer, srcOffset: 0, dst: value, dstOffset: 0, count: length);

            return value;
        }

        public Serializer ToSerializable()
        {
            var bytes = ToByteArray();
            return new Serializer { Buffer = bytes, length = bytes.Length, Capacity = bytes.Length };
        }

        public char[] ToCharArray() => Encoding.GetChars(buffer, 0, length);

        public override string ToString() => Encoding.GetString(buffer, 0, length);

        object InternalToObject<T>(object value, IBackingSerializer serializer)
        {
            var currentPosition = position;
            position = 0;

            value = InternalDeserializeObject<T>(value, raw: true, serializer: serializer);

            position = currentPosition;
            return value;
        }

        public object ToObject(Type type)
        {
            var currentPosition = position;
            position = 0;

            var value = TypeDeserializeObject(type, raw: true);

            position = currentPosition;
            return value;
        }

        public T ToObject<T>(T value) => (T)InternalToObject<T>(value, serializer: null);
        public T ToObject<T>() => (T)InternalToObject<T>(value: null, serializer: null);
        public T ToObject<T>(T value, IBackingSerializer serializer) => (T)InternalToObject<T>(value, serializer);
        public T ToObject<T>(IBackingSerializer serializer) => (T)InternalToObject<T>(value: null, serializer: serializer);

#endregion ToConversions

#region Serialize

#region SerializeNumeric

#if BRIDGE_NET
        void SerializeNumeric(ValueType value, int size, bool unsigned, bool floatingPoint = false)
        {

        }
#else
#if DEBUG && !NET40 && !NET35 && !NET20
        void SerializeNumeric(ValueType value, int size, bool unsigned, bool floatingPoint = false, bool nullable = false, [CallerMemberName] string callerName = "")
#else
        void SerializeNumeric(ValueType value, int size, bool unsigned, bool floatingPoint = false)
#endif
        {
            EnsureCapacity(size, SerializerOperation.Serialize);

            unsafe
            {
                fixed (byte* ptr = &(buffer[position]))
                {
                    var sizeError = false;

                    var swap = BitConverter.IsLittleEndian && ByteOrder == ByteOrder.BigEndian ||
                        !BitConverter.IsLittleEndian && ByteOrder == ByteOrder.LittleEndian;

                    if (unsigned)
                        switch (size)
                        {
                            case sizeof(byte): *((byte*)ptr) = (byte)value; break;
                            case sizeof(ushort): *((ushort*)ptr) = swap ? Utilities.ByteOrder.Swap((ushort)value) : (ushort)value; break;
                            case sizeof(uint): *((uint*)ptr) = swap ? Utilities.ByteOrder.Swap((uint)value) : (uint)value; break;
                            case sizeof(ulong): *((ulong*)ptr) = swap ? Utilities.ByteOrder.Swap((ulong)value) : (ulong)value; break;

                            default: sizeError = true; break;
                        }
                    else
                        switch (size)
                        {
                            case sizeof(sbyte): *((sbyte*)ptr) = (sbyte)value; break;
                            case sizeof(short): *((short*)ptr) = swap ? Utilities.ByteOrder.Swap((short)value) : (short)value; break;
                            case sizeof(int):
                                {
                                    if (floatingPoint)
                                        *((float*)ptr) = swap ? Utilities.ByteOrder.Swap((float)value) : (float)value;
                                    else
                                        *((int*)ptr) = swap ? Utilities.ByteOrder.Swap((int)value) : (int)value;

                                    break;
                                }
                            case sizeof(long):
                                {
                                    if (floatingPoint)
                                        *((double*)ptr) = swap ? Utilities.ByteOrder.Swap((double)value) : (double)value;
                                    else
                                        *((long*)ptr) = swap ? Utilities.ByteOrder.Swap((long)value) : (long)value;

                                    break;
                                }
                            case sizeof(decimal): *((decimal*)ptr) = swap ? Utilities.ByteOrder.Swap((decimal)value) : (decimal)value; break;

                            default: sizeError = true; break;
                        }

                    if (sizeError)
#if DEBUG && !NET40 && !NET35 && !NET20
                        throw new ArgumentException(string.Format(Utilities.ResourceStrings.ExceptionFormat3, nameof(Serializer),
                            nameof(SerializeNumeric), size, floatingPoint, callerName, Utilities.ResourceStrings.ExceptionMessageBufferDeserializeNumeric));
#else
                        throw new ArgumentException(string.Format(Utilities.ResourceStrings.ExceptionFormat1, nameof(Serializer),
                            nameof(SerializeNumeric), size, floatingPoint, Utilities.ResourceStrings.ExceptionMessageBufferDeserializeNumeric));
#endif
                }
            }

            position += size;
        }
#endif

        public void Serialize(bool value) => SerializeNumeric((byte)(value ? 1 : 0), sizeof(bool), unsigned: true);
        public void Serialize(byte value) => SerializeNumeric(value, sizeof(byte), unsigned: true);
        public void Serialize(short value) => SerializeNumeric(value, sizeof(short), unsigned: false);

        public void Serialize(float value) => SerializeNumeric(value, sizeof(float), unsigned: false, floatingPoint: true);
        public void Serialize(double value) => SerializeNumeric(value, sizeof(double), unsigned: false, floatingPoint: true);
        public void Serialize(decimal value) => SerializeNumeric(value, sizeof(decimal), unsigned: false, floatingPoint: true);

        public void Serialize(sbyte value) => SerializeNumeric(value, sizeof(sbyte), unsigned: false);
        public void Serialize(ushort value) => SerializeNumeric(value, sizeof(ushort), unsigned: true);
        public void Serialize(uint value) => SerializeNumeric(value, sizeof(uint), unsigned: true);
        public void Serialize(ulong value) => SerializeNumeric(value, sizeof(ulong), unsigned: true);

        public void Serialize(char value) => Serialize((ushort)value);
        public void Serialize(int value) => SerializeCompressedInt((uint)(value + CompressedIntThreshold));
        public void Serialize(long value) => SerializeCompressedInt((ulong)(value + CompressedIntThreshold));

        public void Serialize(Guid value) => SerializeRaw(value.ToByteArray());
        public void Serialize(BitSerializer value) => Serialize((long)value);

        public void Serialize(TimeSpan value) => Serialize(value.Ticks);
        public void Serialize(DateTime value) => Serialize(value.Ticks);

#if !BRIDGE_NET
        public void Serialize(DateTimeOffset value)
        {
            Serialize(value.DateTime);
            Serialize(value.Offset);
        }
#endif

        public void Serialize(Enum value) => Serialize(Convert.ToInt64(value));

#region Nullable

        delegate void SerializeSignature<T>(T value) where T : struct;

        void SerializeNullableNumeric<T>(T? value, SerializeSignature<T> Serialize) where T : struct
        {
            if (value == null)
                this.Serialize(false);
            else
            {
                this.Serialize(true);
                Serialize((T)value);
            }
        }

        public void Serialize(bool? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(byte? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(short? value) => SerializeNullableNumeric(value, Serialize);

        public void Serialize(float? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(double? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(decimal? value) => SerializeNullableNumeric(value, Serialize);

        public void Serialize(sbyte? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(ushort? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(uint? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(ulong? value) => SerializeNullableNumeric(value, Serialize);

        public void Serialize(char? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(int? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(long? value) => SerializeNullableNumeric(value, Serialize);

        public void Serialize(Guid? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(BitSerializer? value) => SerializeNullableNumeric(value, Serialize);

        public void Serialize(TimeSpan? value) => SerializeNullableNumeric(value, Serialize);
        public void Serialize(DateTime? value) => SerializeNullableNumeric(value, Serialize);

#if !BRIDGE_NET
        public void Serialize(DateTimeOffset? value) => SerializeNullableNumeric(value, Serialize);
#endif

        public void Serialize<T>(T? value) where T : struct
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
                SerializeNullableNumeric(value, Serialize);
            else
            {
                if (value == null)
                    Serialize(false);
                else
                {
                    Serialize(true);
                    Serialize(Convert.ToInt64(value));
                }
            }
        }

#endregion Nullable

#region CompressedInt

        public void SerializeCompressedInt(short value) => SerializeCompressedInt((ulong)((value << 1) ^ (value >> 15)));
        public void SerializeCompressedInt(int value) => SerializeCompressedInt((ulong)((value << 1) ^ (value >> 31)));
        public void SerializeCompressedInt(long value) => SerializeCompressedInt((ulong)((value << 1) ^ (value >> 63)));

        public void SerializeCompressedInt(ushort value) => SerializeCompressedInt((ulong)value);
        public void SerializeCompressedInt(uint value) => SerializeCompressedInt((ulong)value);
        public void SerializeCompressedInt(ulong value)
        {
            while (value >= 0x80)
            {
                Serialize((byte)(value | 0x80));
                value >>= 7;
            }

            Serialize((byte)value);
        }

#endregion CompressedInt

#endregion SerializeNumeric

#region Uri

        public void Serialize(Uri value) => Serialize(value.ToString());
        public void SerializeRaw(Uri value) => SerializeRaw(value.ToString());

#endregion Uri

#region string

        void InternalSerialize(string value, bool raw)
        {
            if (!string.IsNullOrEmpty(value))
            {
#if !BRIDGE_NET
                unsafe
                {
                    fixed (char* ptr = value)
                        InternalSerialize(ptr, value.Length, 0, value.Length, false, false, raw);
                }
#else
                InternalSerialize(value.ToCharArray(), 0, value.Length, wide: false, raw: raw);
#endif
            }
            else if (!raw)
                Serialize(0);
        }

        public void Serialize(string value) => InternalSerialize(value, AutoRaw);
        public void SerializeRaw(string value) => InternalSerialize(value, raw: true);

#endregion

#region SecureString

        //unsafe void InternalSerialize(SecureString value, bool raw)
        //{
        //    if (value?.Length > 0)
        //    {
        //        var ptr = Utilities.Marshal.SecureStringToCoTaskMemUnicode(value);
        //        InternalSerialize((char*)ptr, value.Length, 0, value.Length, false, false, raw);
        //        Utilities.Marshal.ZeroFreeCoTaskMemUnicode(ptr);
        //    }
        //    else if (!raw)
        //        Serialize(0);
        //}

        //public void Serialize(SecureString value) => InternalSerialize(value, raw: AutoRaw);
        //public void SerializeRaw(SecureString value) => InternalSerialize(value, raw: true);

#endregion SecureString

#region Serializer

        void InternalSerialize(Serializer value, bool raw)
        {
            if (value?.length > 0)
                InternalSerialize(value.buffer, 0, value.length, raw);
            else if (!raw)
                Serialize(0);
        }

        public void Serialize(Serializer value) => InternalSerialize(value, AutoRaw);
        public void SerializeRaw(Serializer value) => InternalSerialize(value, raw: true);

#endregion

#region MemoryStream

        void InternalSerialize(MemoryStream value, bool raw)
        {
            if (value?.Length > 0)
            {
                if (value.Length > int.MaxValue)
                    throw new BufferOverflowException();

                InternalSerialize(value.GetBuffer(), 0, (int)value.Length, raw);
            }
            else if (!raw)
                Serialize(0);
        }

        public void Serialize(MemoryStream value) => InternalSerialize(value, AutoRaw);
        public void SerializeRaw(MemoryStream value) => InternalSerialize(value, raw: true);

#endregion

#region SerializeEnumerable

        void InternalSerialize<T, TValue>(IEnumerable<T> value1, IEnumerable<KeyValuePair<T, TValue>> value2)
        {
            var value = (IEnumerable)value1 ?? value2;

            var count = 0;

            if (value != null)
                foreach (var item in value)
                    count++;

            Serialize(count);

            if (count == 0)
                return;

            if (value == value1)
                foreach (var item in value1)
                    TypeSerializeObject(typeof(T), item, raw: false);
            else
                foreach (var item in value2)
                {
                    TypeSerializeObject(typeof(T), item.Key, raw: false);
                    TypeSerializeObject(typeof(TValue), item.Value, raw: false);
                }
        }

        public void Serialize<T>(T[] value) => InternalSerialize<T, T>(value, null);
        public void Serialize<T>(IEnumerable<T> value) => InternalSerialize<T, T>(value, null);
        public void Serialize<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value) => InternalSerialize<TKey, TValue>(null, value);

        //public void SerializeRaw<T>(T[] value) => InternalSerialize<T, T>(value, null, raw: true);
        //public void SerializeRaw<T>(IEnumerable<T> value) => InternalSerialize<T, T>(value, null, raw: true);
        //public void SerializeRaw<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value) => InternalSerialize<TKey, TValue>(null, value, raw: true);

#endregion SerializeEnumerable

#region Object / Serializable

        void TypeSerializeObject(Type type, object value, bool raw)
        {
            AutoRaw = raw;
            //GetSerializationMethod(type, SerializerOperation.Serialize).Invoke(this, new object[] { value });
            //GetSerializationMethodAction(type, SerializerOperation.Serialize)(this, value);
            Reflector.Delegate.GetAction(type)(this, value);
            AutoRaw = false;
        }

        void InternalSerializeObject<T>(T value, bool raw, IBackingSerializer serializer = null)
        {
            AutoRaw = false;

            if (serializer != null)
            {
                serializer.Serialize(value, this, rawValue: raw);
                return;
            }

            if (value == null)
            {
                if (!raw)
                    Serialize(0);

                return;
            }

            var type = value.GetType();
            var serializable = value as ISerializable;

            if (serializable == null && Reflector.IsKnownType(type))
            {
                TypeSerializeObject(type, value, raw);
                return;
            }

            if (CircularReferencesActive)
            {
                if (!raw)
                {
                    if (CircularReferencesDictionary.TryGetValue(value, out var index))
                    {
                        Serialize((index + 1) * (-1));
                        return;
                    }
                }

                //index = CircularReferencesIndex++;
                CircularReferencesDictionary.Add(value, CircularReferencesIndex++);
                //CircularReferencesIndexDictionary.Add(index, value);
            }

            if (UseObjectSerialization)
            {
                if (SerializationStack.Count == 0)
                    ObjectSerializationActive = true;

                var objSerial = SerializationStack.Count > 0 ? SerializationStack.Peek() : null;
                SerializationStack.Push(new ObjectSerialization(position, raw, previous: objSerial));
            }

            try
            {
                if (!raw && UseObjectSerialization)
                    Serialize(0);
                else if (!raw)
                    Serialize(1);

                if (serializable != null)
                    serializable.Serialize(this);
                else
                {
                    var typeData = default(Reflector.TypeData);
                    var prevTypeData = default(Reflector.TypeData);

                    while (type != typeof(ValueType) && type != typeof(object))
                    {
                        if (typeData == null)
                        {
                            if (!Reflector.TypesCache.TryGetValue(type, out typeData))
                            {
                                typeData = new Reflector.TypeData(type);
                                Reflector.TypesCache.TryAdd(type, typeData);
                            }

                            if (prevTypeData != null)
                                prevTypeData.Parent = typeData;
                        }

                        var serializedFieldsCount = 0;

                        for (var i = 0; i < typeData.Fields.Length; i++)
                            if (typeData.Fields[i].ShouldSerialize)
                            {
                                var fieldValue = typeData.Fields[i].GetValue(value);

                                TypeSerializeObject(typeData.Fields[i].Field.FieldType, fieldValue, raw: false);
                                serializedFieldsCount++;
                            }

                        if (serializedFieldsCount == 0)
                            Serialize((byte)0);

                        prevTypeData = typeData;
                        typeData = typeData.Parent ?? null;
                        type = typeData?.Parent?.Type ?? type.GetTypeInfo().BaseType;
                    }
                }

                if (!raw && UseObjectSerialization)
                {
                    var finalPosition = position;
                    var serializationObject = SerializationStack.Peek();
                    position = serializationObject.BufferPosition;
                    Serialize(finalPosition - (position + serializationObject.PositionLenght));
                    position = finalPosition;
                }
            }
            catch (Exception ex) when (!(ex is BufferOverflowException))
            {
                throw DataException();
            }
            finally
            {
                if (UseObjectSerialization)
                {
                    var objSerial = SerializationStack.Pop();

                    if (objSerial.Previous != null)
                    {
                        objSerial.Previous.Next = null;
                        objSerial.Previous = null;
                    }

                    if (SerializationStack.Count == 0)
                    {
                        if (CircularReferencesActive)
                        {
                            CircularReferencesIndex = 0;
                            CircularReferencesDictionary.Clear();
                            //CircularReferencesIndexDictionary.Clear();
                        }

                        SerializationStack.TrimExcess();

                        ObjectSerializationActive = false;
                    }
                }
            }
        }

        //public static TDestination Map<TSource, TDestination>(TSource sourceValue)
        //    => (TDestination)Map(sourceValue, destinationValue: null, typeof(TDestination));

        //public static object Map<TSource>(TSource value, object destinationValue, Type destinationType, Serializer serializer = null)
        //public static object Map<TSource>(TSource value, Type destinationType, Dictionary<object, object> circularReferencesDictionary = null, Serializer serializer = null)

        /// <summary>
        /// Simple and slow mapper
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <param name="circularReferencesDictionary"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static object Map<TSource>(TSource value, Type destinationType, Dictionary<object, object> circularReferencesDictionary = default, Serializer serializer = null)
        {
            if (value == null)
                return null;

            var type = value.GetType();

            if (Reflector.IsKnownType(type))
            {
                serializer = serializer ?? new Serializer();
                serializer.Position = 0;
                serializer.Serialize(value);
                serializer.Position = 0;
                return serializer.DeserializeObject(destinationType);
            }

            circularReferencesDictionary = circularReferencesDictionary ?? new Dictionary<object, object>();

            if (circularReferencesDictionary.TryGetValue(value, out var destination))
                return destination;

            var destinationValue = Activator.CreateInstance(destinationType);

            circularReferencesDictionary.Add(value, destinationValue);

            try
            {
                var typeData = default(Reflector.TypeData);
                var prevTypeData = default(Reflector.TypeData);

                var typeDataDest = default(Reflector.TypeData);
                var prevTypeDataDest = default(Reflector.TypeData);

                while (type != typeof(ValueType) && type != typeof(object))
                {
                    if (typeData == null)
                    {
                        if (!Reflector.TypesCache.TryGetValue(type, out typeData))
                        {
                            typeData = new Reflector.TypeData(type);
                            Reflector.TypesCache.TryAdd(type, typeData);
                        }

                        if (prevTypeData != null)
                            prevTypeData.Parent = typeData;
                    }

                    if (typeDataDest == null)
                    {
                        if (!Reflector.TypesCache.TryGetValue(destinationType, out typeDataDest))
                        {
                            typeDataDest = new Reflector.TypeData(destinationType);
                            Reflector.TypesCache.TryAdd(destinationType, typeDataDest);
                        }

                        if (prevTypeDataDest != null)
                            prevTypeDataDest.Parent = typeDataDest;
                    }

                    for (var i = 0; i < typeData.Properties.Length; i++)
                        if (typeData.Properties[i].ShouldSerialize)
                        {
                            if (typeDataDest.Properties.SingleOrDefault(p => p.Name == typeData.Properties[i].Name) is Reflector.PropertyData propertyDestData)
                            {
                                var propertyValue = typeData.Properties[i].GetValue(value, index: default);

                                var ok = false;
                                var propertyDestValue = default(object);

                                if (propertyDestData.Property.PropertyType.GetTypeInfo().IsValueType)
                                    if (Reflector.IsKnownType(propertyDestData.Property.PropertyType))
                                    {
                                        ok = true;
                                        propertyDestValue = CheckFieldCollectionType(propertyDestValue, propertyDestData.Property.PropertyType);
                                    }

                                if (!ok)
                                {
                                    propertyDestValue = Map(propertyValue, propertyDestData.Property.PropertyType, circularReferencesDictionary, serializer);
                                    propertyDestValue = CheckFieldCollectionType(propertyDestValue, propertyDestData.Property.PropertyType);
                                }

                                propertyDestData.SetValue(destinationValue, propertyDestValue, index: default);
                            }
                        }

                    prevTypeData = typeData;
                    typeData = typeData.Parent ?? null;
                    type = typeData?.Parent?.Type ?? type.GetTypeInfo().BaseType;
                }
            }
            catch (Exception ex) when (!(ex is BufferOverflowException))
            {
                throw DataException();
            }

            return destinationValue;
        }

        public void Serialize<T>(T value) => InternalSerializeObject(value, raw: false);
        public void SerializeRaw<T>(T value) => InternalSerializeObject(value, raw: true);
        public void Serialize<T>(T value, IBackingSerializer serializer) => InternalSerializeObject(value, raw: false, serializer: serializer);
        public void SerializeRaw<T>(T value, IBackingSerializer serializer) => InternalSerializeObject(value, raw: true, serializer: serializer);

#endregion Object / Serializable

#region byte[]*

        public void Serialize(byte[] value) => InternalSerialize(value, 0, value?.Length ?? 0, raw: AutoRaw);
        public void Serialize(byte[] value, int index, int count) => InternalSerialize(value, index, count, raw: false);

        public void SerializeRaw(byte[] value) => InternalSerialize(value, 0, value?.Length ?? 0, raw: true);
        public void SerializeRaw(byte[] value, int index, int count) => InternalSerialize(value, index, count, raw: true);

#if BRIDGE_NET
        void InternalSerialize(byte[] value, int index, int count, bool raw)
        {

        }
#else
        public unsafe void Serialize(byte* value) => InternalSerialize(value, 0, 0, 0, true, raw: AutoRaw);
        unsafe public void Serialize(byte* value, int count) => InternalSerialize(value, count, 0, count, false, raw: false);

        public unsafe void SerializeRaw(byte* value) => InternalSerialize(value, 0, 0, 0, true, raw: true);
        public unsafe void SerializeRaw(byte* value, int count) => InternalSerialize(value, count, 0, count, false, raw: true);

        unsafe void InternalSerialize(byte[] value, int index, int count, bool raw)
        {
            if (value == null)
                InternalSerialize((byte*)IntPtr.Zero, 0, index, count, calculateLength: false, raw: raw);
            else
                fixed (byte* ptr = value)
                    InternalSerialize(ptr, value.Length, index, count, calculateLength: false, raw: raw);
        }

        unsafe void InternalSerialize(byte* value, int size, int index, int count, bool calculateLength, bool raw)
        {
            if (calculateLength)
                if ((IntPtr)value != IntPtr.Zero)
                    size = count = Utilities.Memory.Strlen(value);

            if ((IntPtr)value == IntPtr.Zero)
                if (raw && size == 0 && index == 0 && count == 0)
                    return;
                else if (raw || size != 0 || index != 0 || count != 0)
                    throw new ArgumentNullException();
                else
                {
                    Serialize(0);
                    return;
                }

            if (index < 0 || count < 0 || size < 0)
                throw new ArgumentOutOfRangeException();

            if (size - index < count)
                throw new ArgumentException();

            var varIntSize = 0;

            if (!raw)
                varIntSize = Utilities.EncodedInteger.RequiredBytes((uint)count);

            if (position + count + varIntSize < 0)
                throw new IOException("Buffer too long.");

            if (!raw)
                Serialize(count);

            if (count == 0)
                return;

            EnsureCapacity(count, SerializerOperation.Serialize);

            fixed (byte* ptr = buffer)
                Utilities.Memory.Memcpy(value + index, ptr + position, count);

            position += count;
        }

#endif

#endregion byte[]*

#region char[]*

        public void Serialize(char[] value) => InternalSerialize(value, 0, (int)value?.Length, wide: false, raw: AutoRaw);
        public void Serialize(char[] value, int index, int count) => InternalSerialize(value, index, count, wide: false, raw: false);

        public void SerializeRaw(char[] value) => InternalSerialize(value, 0, (int)value?.Length, wide: false, raw: true);
        public void SerializeRaw(char[] value, int index, int count) => InternalSerialize(value, index, count, wide: false, raw: true);

#if BRIDGE_NET
        void InternalSerialize(char[] value, int index, int count, bool wide, bool raw)
        {

        }
#else

        public unsafe void Serialize(char* value) => InternalSerialize(value, 0, 0, 0, wide: false, calculateLength: true, raw: AutoRaw);
        public unsafe void Serialize(char* value, int count) => InternalSerialize(value, count, 0, count, wide: false, calculateLength: false, raw: false);

        public unsafe void SerializeRaw(char* value) => InternalSerialize(value, 0, 0, 0, wide: false, calculateLength: true, raw: true);
        public unsafe void SerializeRaw(char* value, int count) => InternalSerialize(value, count, 0, count, wide: false, calculateLength: false, raw: true);

        unsafe void InternalSerialize(char[] value, int index, int count, bool wide, bool raw)
        {
            if (value == null)
                InternalSerialize((char*)IntPtr.Zero, 0, index, count, wide, false, raw);
            else
                fixed (char* ptr = value)
                    InternalSerialize(ptr, value.Length, index, count, wide, false, raw);
        }

        unsafe void InternalSerialize(char* value, int size, int index, int count, bool wide, bool calculateLength, bool raw)
        {
            if (calculateLength)
                if ((IntPtr)value != IntPtr.Zero)
                    size = count = Utilities.Memory.Wcslen(value);

            if ((IntPtr)value == IntPtr.Zero)
                if (raw && size == 0 && index == 0 && count == 0)
                    return;
                else if (raw || size != 0 || index != 0 || count != 0)
                    throw new ArgumentNullException();
                else
                {
                    Serialize(0);
                    return;
                }

            if (index < 0 || count < 0 || size < 0)
                throw new ArgumentOutOfRangeException();

            if (size - index < count)
                throw new ArgumentException();

            var varIntSize = 0;

            if (wide)
            {
                if (!raw)
                    varIntSize = Utilities.EncodedInteger.RequiredBytes((uint)(count * 2));

                if (position + (count * 2) + varIntSize < 0)
                    throw new IOException("Buffer too long.");

                if (!raw)
                    Serialize(count * 2);

                if (count == 0)
                    return;

                EnsureCapacity(count * 2, SerializerOperation.Serialize);

                fixed (byte* ptr = buffer)
                    Utilities.Memory.Wstrcpy(value + index, (char*)(ptr + position), count);

                position += count * 2;

                return;
            }

#if !NETSTANDARD1_0
            var byteCount = Encoding.GetByteCount(value + index, count);
#else
            var managedValue = new char[count];
            fixed (char* charPtr = managedValue)
                Utilities.Memory.Wstrcpy(value + index, charPtr, count);
            var byteCount = Encoding.GetByteCount(managedValue);
#endif

            if (!raw)
                varIntSize = Utilities.EncodedInteger.RequiredBytes((uint)byteCount);

            if (position + byteCount + varIntSize < 0)
                throw new IOException("Buffer too long.");

            if (!raw)
                Serialize(byteCount);

            if (count == 0)
                return;

            var previousLength = length;

            EnsureCapacity(byteCount, SerializerOperation.Serialize);

            var realByteCount = 0;

#if !NETSTANDARD1_0
            fixed (byte* ptr = buffer)
                realByteCount = Encoding.GetBytes(value + index, count, ptr + position, byteCount);
#else
            realByteCount = Encoding.GetBytes(managedValue, 0, count, Buffer, position);
#endif

            if (!raw)
                if (realByteCount != byteCount)
                {
                    var realVarIntSize = Utilities.EncodedInteger.RequiredBytes((uint)realByteCount);

                    position -= varIntSize;
                    Serialize(realByteCount);

                    var diff = varIntSize - realVarIntSize;

                    if (diff > 0)
                        fixed (byte* ptr = &buffer[position + diff])
                            Utilities.Memory.Memcpy(ptr, ptr - diff, realByteCount);
                }

            position += realByteCount;

            if (position > previousLength)
                length = position;
        }

#endif

#endregion char[]*

#endregion Serialize

#region Deserialize

#region DeserializeNumeric

#if BRIDGE_NET
        T DeserializeNumeric<T>(int size, bool unsigned, bool floatingPoint = false) where T : struct
        {
            return default(T);
        }
#else
#if DEBUG && !NET40 && !NET35 && !NET20
        T DeserializeNumeric<T>(int size, bool unsigned, bool floatingPoint = false, [CallerMemberName] string callerName = "") where T : struct
#else
        T DeserializeNumeric<T>(int size, bool unsigned, bool floatingPoint = false) where T : struct
#endif
        {
            EnsureCapacity(size, SerializerOperation.Deserialize);

            var value = default(ValueType);

            unsafe
            {
                fixed (byte* ptr = &(buffer[position]))
                {
                    position += size;

                    var swap = BitConverter.IsLittleEndian && ByteOrder == ByteOrder.BigEndian ||
                        !BitConverter.IsLittleEndian && ByteOrder == ByteOrder.LittleEndian;

                    if (unsigned)
                        switch (size)
                        {
                            case sizeof(byte): return (T)(value = *(byte*)ptr);
                            case sizeof(ushort): return (T)(value = swap ? Utilities.ByteOrder.Swap(*(ushort*)ptr) : *(ushort*)ptr);
                            case sizeof(uint): return (T)(value = swap ? Utilities.ByteOrder.Swap(*(uint*)ptr) : *(uint*)ptr);
                            case sizeof(ulong): return (T)(value = swap ? Utilities.ByteOrder.Swap(*(ulong*)ptr) : *(ulong*)ptr);
                        }
                    else
                        switch (size)
                        {
                            case sizeof(sbyte): return (T)(value = *(sbyte*)ptr);
                            case sizeof(short): return (T)(value = swap ? Utilities.ByteOrder.Swap(*(short*)ptr) : *(short*)ptr);
                            case sizeof(int): return (T)(value = swap ? Utilities.ByteOrder.Swap(floatingPoint ? *(float*)ptr : *(int*)ptr) : floatingPoint ? *(float*)ptr : *(int*)ptr);
                            case sizeof(long): return (T)(value = swap ? Utilities.ByteOrder.Swap(floatingPoint ? *(double*)ptr : *(long*)ptr) : floatingPoint ? *(double*)ptr : *(long*)ptr);
                            case sizeof(decimal): return (T)(value = swap ? Utilities.ByteOrder.Swap(*(decimal*)ptr) : *(decimal*)ptr);
                        }
                }
            }

#if DEBUG && !NET40 && !NET35 && !NET20
            throw new ArgumentException(string.Format(Utilities.ResourceStrings.ExceptionFormat3, nameof(Serializer),
                nameof(DeserializeNumeric), size, floatingPoint, callerName, Utilities.ResourceStrings.ExceptionMessageBufferDeserializeNumeric));
#else
            throw new ArgumentException(string.Format(Utilities.ResourceStrings.ExceptionFormat3, nameof(Serializer),
                nameof(DeserializeNumeric), size, unsigned, floatingPoint, Utilities.ResourceStrings.ExceptionMessageBufferDeserializeNumeric));
#endif
        }
#endif

        public bool DeserializeBoolean() => DeserializeNumeric<byte>(sizeof(bool), unsigned: true) != 0;
        public byte DeserializeByte() => DeserializeNumeric<byte>(sizeof(byte), unsigned: true);
        public short DeserializeInt16() => DeserializeNumeric<short>(sizeof(short), unsigned: false);

        public float DeserializeSingle() => DeserializeNumeric<float>(sizeof(float), unsigned: false, floatingPoint: true);
        public double DeserializeDouble() => DeserializeNumeric<double>(sizeof(double), unsigned: false, floatingPoint: true);
        public decimal DeserializeDecimal() => DeserializeNumeric<decimal>(sizeof(decimal), unsigned: false, floatingPoint: true);

        public sbyte DeserializeSByte() => DeserializeNumeric<sbyte>(sizeof(sbyte), unsigned: false);
        public ushort DeserializeUInt16() => DeserializeNumeric<ushort>(sizeof(ushort), unsigned: true);
        public uint DeserializeUInt32() => DeserializeNumeric<uint>(sizeof(uint), unsigned: true);
        public ulong DeserializeUInt64() => DeserializeNumeric<ulong>(sizeof(ulong), unsigned: true);

        public char DeserializeChar() => (char)DeserializeCompressedUInt16();
        public int DeserializeInt32() => (int)(DeserializeCompressedUInt32()) - CompressedIntThreshold;
        public long DeserializeInt64() => (long)(DeserializeCompressedUInt64()) - CompressedIntThreshold;

        public Guid DeserializeGuid() => new Guid(DeserializeBytes(count: 16));
        public BitSerializer DeserializeBitSerializer() => DeserializeInt64();

        public TimeSpan DeserializeTimeSpan() => new TimeSpan(DeserializeInt64());
        public DateTime DeserializeDateTime() => new DateTime(DeserializeInt64());

#if !BRIDGE_NET
        public DateTimeOffset DeserializeDateTimeOffset() => new DateTimeOffset(DeserializeDateTime(), DeserializeTimeSpan());

        public T DeserializeEnum<T>() where T : struct => (T)Enum.Parse(typeof(T), DeserializeInt64().ToString());
#else
        public T DeserializeEnum<T>() where T : struct => (T)(object)Enum.Parse(typeof(T), DeserializeInt64().ToString());
#endif

#region Nullable

        delegate T DeserializeSignature<T>();

        T? DeserializeNullableNumeric<T>(DeserializeSignature<T> Deserialize) where T : struct
        {
            if (!DeserializeBoolean())
                return default;

            return Deserialize();
        }

        public bool? DeserializeNullableBoolean() => DeserializeNullableNumeric(DeserializeBoolean);
        public byte? DeserializeNullableByte() => DeserializeNullableNumeric(DeserializeByte);
        public short? DeserializeNullableInt16() => DeserializeNullableNumeric(DeserializeInt16);

        public float? DeserializeNullableSingle() => DeserializeNullableNumeric(DeserializeSingle);
        public double? DeserializeNullableDouble() => DeserializeNullableNumeric(DeserializeDouble);
        public decimal? DeserializeNullableDecimal() => DeserializeNullableNumeric(DeserializeDecimal);

        public sbyte? DeserializeNullableSByte() => DeserializeNullableNumeric(DeserializeSByte);
        public ushort? DeserializeNullableUInt16() => DeserializeNullableNumeric(DeserializeUInt16);
        public uint? DeserializeNullableUInt32() => DeserializeNullableNumeric(DeserializeUInt32);
        public ulong? DeserializeNullableUInt64() => DeserializeNullableNumeric(DeserializeUInt64);

        public char? DeserializeNullableChar() => DeserializeNullableNumeric(DeserializeChar);
        public int? DeserializeNullableInt32() => DeserializeNullableNumeric(DeserializeInt32);
        public long? DeserializeNullableInt64() => DeserializeNullableNumeric(DeserializeInt64);

        public Guid? DeserializeNullableGuid() => DeserializeNullableNumeric(DeserializeGuid);
        public BitSerializer? DeserializeNullableBitSerializer() => DeserializeNullableNumeric(DeserializeBitSerializer);

        public TimeSpan? DeserializeNullableTimeSpan() => DeserializeNullableNumeric(DeserializeTimeSpan);
        public DateTime? DeserializeNullableDateTime() => DeserializeNullableNumeric(DeserializeDateTime);

#if !BRIDGE_NET
        public DateTimeOffset? DeserializeNullableDateTimeOffset() => DeserializeNullableNumeric(DeserializeDateTimeOffset);
#endif

        public T? DeserializeNullableT<T>() where T : struct
        {
            if (typeof(T).GetTypeInfo().IsEnum)
                return DeserializeNullableNumeric(DeserializeEnum<T>);

            return DeserializeNullableNumeric(DeserializeObject<T>);
        }

#endregion Nullable

#region Native Types TryDeserialize

#if BRIDGE_NET
        bool TryDeserializeNumeric<T>(out T value, int size, bool unsigned, bool floatingPoint = false) where T : struct
        {
            value = default(T);
            return false;
        }
#else
        bool TryDeserializeNumeric<T>(out T value, int size, bool unsigned, bool floatingPoint = false) where T : struct
        {
            var result = false;
            var valueType = default(ValueType);

            unsafe
            {
                if (!(length - position < size))
                {
                    result = true;

                    fixed (byte* ptr = &(buffer[position]))
                    {
                        position += size;

                        var swap = BitConverter.IsLittleEndian && ByteOrder == ByteOrder.BigEndian ||
                            !BitConverter.IsLittleEndian && ByteOrder == ByteOrder.LittleEndian;

                        if (unsigned)
                            switch (size)
                            {
                                case sizeof(byte): valueType = *(byte*)ptr; break;
                                case sizeof(ushort): valueType = swap ? Utilities.ByteOrder.Swap(*(ushort*)ptr) : *(ushort*)ptr; break;
                                case sizeof(uint): valueType = swap ? Utilities.ByteOrder.Swap(*(uint*)ptr) : *(uint*)ptr; break;
                                case sizeof(ulong): valueType = swap ? Utilities.ByteOrder.Swap(*(ulong*)ptr) : *(ulong*)ptr; break;
                            }
                        else
                            switch (size)
                            {
                                case sizeof(sbyte): valueType = *(sbyte*)ptr; break;
                                case sizeof(short): valueType = swap ? Utilities.ByteOrder.Swap(*(short*)ptr) : *(short*)ptr; break;
                                case sizeof(int): valueType = swap ? Utilities.ByteOrder.Swap(floatingPoint ? *(float*)ptr : *(int*)ptr) : floatingPoint ? *(float*)ptr : *(int*)ptr; break;
                                case sizeof(long): valueType = swap ? Utilities.ByteOrder.Swap(floatingPoint ? *(double*)ptr : *(long*)ptr) : floatingPoint ? *(double*)ptr : *(long*)ptr; break;
                                case sizeof(decimal): valueType = swap ? Utilities.ByteOrder.Swap(*(decimal*)ptr) : *(decimal*)ptr; break;
                            }
                    }
                }
            }

            value = result ? (T)valueType : default;
            return result;
        }
#endif

        public bool TryDeserializeByte(out byte value) => TryDeserializeNumeric(out value, sizeof(byte), unsigned: true);
        public bool TryDeserializeInt16(out short value) => TryDeserializeNumeric(out value, sizeof(short), unsigned: false);

        public bool TryDeserializeSingle(out float value) => TryDeserializeNumeric(out value, sizeof(float), unsigned: false);
        public bool TryDeserializeDouble(out double value) => TryDeserializeNumeric(out value, sizeof(double), unsigned: false);
        public bool TryDeserializeDecimal(out decimal value) => TryDeserializeNumeric(out value, sizeof(decimal), unsigned: false);

        public bool TryDeserializeSByte(out sbyte value) => TryDeserializeNumeric(out value, sizeof(sbyte), unsigned: false);
        public bool TryDeserializeUInt16(out ushort value) => TryDeserializeNumeric(out value, sizeof(ushort), unsigned: true);
        public bool TryDeserializeUInt32(out uint value) => TryDeserializeNumeric(out value, sizeof(uint), unsigned: true);
        public bool TryDeserializeUInt64(out ulong value) => TryDeserializeNumeric(out value, sizeof(ulong), unsigned: true);

        public bool TryDeserializeBoolean(out bool value)
        {
            value = default;

            if (!TryDeserializeByte(out var bValue))
                return false;

            value = bValue != 0;
            return true;
        }

        public bool TryDeserializeChar(out char value)
        {
            value = default;

            if (!TryDeserializeCompressedUInt16(out var uvalue))
                return false;

            value = (char)uvalue;
            return true;
        }

        public bool TryDeserializeInt32(out int value)
        {
            value = default;

            if (!TryDeserializeCompressedUInt32(out var uvalue))
                return false;

            value = (int)uvalue - CompressedIntThreshold;
            return true;
        }

        public bool TryDeserializeInt64(out long value)
        {
            value = default;

            if (!TryDeserializeCompressedUInt64(out var uvalue))
                return false;

            value = (long)uvalue - CompressedIntThreshold;
            return true;
        }

        public bool TryDeserializeGuid(out Guid value)
        {
            value = Guid.Empty;

            if (!TryDeserializeBytes(out var bytes, count: 16))
                return false;

            value = new Guid(bytes);
            return true;
        }

        public bool TryDeserializeBitSerializer(out BitSerializer value)
        {
            value = default;

            if (!TryDeserializeInt64(out var lValue))
                return false;

            value = new BitSerializer(lValue);
            return true;
        }

        public bool TryDeserializeTimeSpan(out TimeSpan value)
        {
            value = default;

            if (!TryDeserializeInt64(out var lValue))
                return false;

            value = new TimeSpan(lValue);
            return true;
        }

        public bool TryDeserializeDateTime(out DateTime value)
        {
            value = default;

            if (!TryDeserializeInt64(out var lValue))
                return false;

            value = new DateTime(lValue);
            return true;
        }

#if !BRIDGE_NET
        public bool TryDeserializeDateTimeOffset(out DateTimeOffset value)
        {
            value = default;

            if (TryDeserializeDateTime(out var dateTime))
            {
                if (TryDeserializeTimeSpan(out var timeSpan))
                {
                    value = new DateTimeOffset(dateTime, timeSpan);
                    return true;
                }
                else
                    position -= sizeof(long);
            }

            return false;
        }
#endif

        public bool TryDeserializeEnum<TEnum>(out TEnum value) where TEnum : struct
        {
            value = default;

            if (!TryDeserializeInt64(out var lValue))
                return false;

#if NET20 || NET35
            var result = Enum.IsDefined(typeof(TEnum), lValue);
            value = (TEnum)Enum.ToObject(typeof(TEnum), lValue);
#else
            var result = Enum.TryParse(lValue.ToString(), out value);
#endif

            if (!result)
                position -= sizeof(long);

            return result;
        }

        public bool TryDeserializeChars(out char[] value, int count)
        {
            value = default;

            if (length - position < count)
                return false;

            value = DeserializeChars(count);
            return true;
        }

        public bool TryDeserializeBytes(out byte[] value, int count)
        {
            value = default;

            if (length - position < count)
                return false;

            value = DeserializeBytes(count);
            return true;
        }

#region Nullable

        delegate bool TryDeserializeSignature<T>(out T value);

        bool TryDeserializeNullableNumeric<T>(out T? value, TryDeserializeSignature<T> TryDeserialize) where T : struct
        {
            var result = false;
            value = default;

            if (result = TryDeserializeBoolean(out var notNull))
                if (notNull)
                    if (result = TryDeserialize(out var nonNullableValue))
                        value = nonNullableValue;

            return result;
        }

        public bool TryDeserializeNullableBoolean(out bool? value) => TryDeserializeNullableNumeric(out value, TryDeserializeBoolean);
        public bool TryDeserializeNullableByte(out byte? value) => TryDeserializeNullableNumeric(out value, TryDeserializeByte);
        public bool TryDeserializeNullableInt16(out short? value) => TryDeserializeNullableNumeric(out value, TryDeserializeInt16);

        public bool TryDeserializeNullableSingle(out float? value) => TryDeserializeNullableNumeric(out value, TryDeserializeSingle);
        public bool TryDeserializeNullableDouble(out double? value) => TryDeserializeNullableNumeric(out value, TryDeserializeDouble);
        public bool TryDeserializeNullableDecimal(out decimal? value) => TryDeserializeNullableNumeric(out value, TryDeserializeDecimal);

        public bool TryDeserializeNullableSByte(out sbyte? value) => TryDeserializeNullableNumeric(out value, TryDeserializeSByte);
        public bool TryDeserializeNullableUInt16(out ushort? value) => TryDeserializeNullableNumeric(out value, TryDeserializeUInt16);
        public bool TryDeserializeNullableUInt32(out uint? value) => TryDeserializeNullableNumeric(out value, TryDeserializeUInt32);
        public bool TryDeserializeNullableUInt64(out ulong? value) => TryDeserializeNullableNumeric(out value, TryDeserializeUInt64);

        public bool TryDeserializeNullableChar(out char? value) => TryDeserializeNullableNumeric(out value, TryDeserializeChar);
        public bool TryDeserializeNullableInt32(out int? value) => TryDeserializeNullableNumeric(out value, TryDeserializeInt32);
        public bool TryDeserializeNullableInt64(out long? value) => TryDeserializeNullableNumeric(out value, TryDeserializeInt64);

        public bool TryDeserializeNullableGuid(out Guid? value) => TryDeserializeNullableNumeric(out value, TryDeserializeGuid);
        public bool TryDeserializeNullableBitSerializer(out BitSerializer? value) => TryDeserializeNullableNumeric(out value, TryDeserializeBitSerializer);

        public bool TryDeserializeNullableTimeSpan(out TimeSpan? value) => TryDeserializeNullableNumeric(out value, TryDeserializeTimeSpan);
        public bool TryDeserializeNullableDateTime(out DateTime? value) => TryDeserializeNullableNumeric(out value, TryDeserializeDateTime);

#if !BRIDGE_NET
        public bool TryDeserializeNullableDateTimeOffset(out DateTimeOffset? value) => TryDeserializeNullableNumeric(out value, TryDeserializeDateTimeOffset);
#endif

        public bool TryDeserializeNullableEnum<T>(out T? value) where T : struct => TryDeserializeNullableNumeric(out value, TryDeserializeEnum);

#endregion

#endregion Native Types Try

#region DeserializeCompressedInt

        public ulong DeserializeCompressedInt(int size, bool signed)
        {
            ulong val1 = 0;
            var val2 = 0;
            byte val3;

            var bitVal = size == sizeof(short) ? 21 : size == sizeof(int) ? 35 : size == sizeof(long) ? 63 : 0;

            if (bitVal == 0)
                throw new ArgumentException(string.Format(Utilities.ResourceStrings.ExceptionFormat, nameof(Serializer),
                    nameof(DeserializeCompressedInt), size, Utilities.ResourceStrings.ExceptionMessageBufferDeserializeNumeric));

            while (val2 != bitVal)
            {
                val3 = DeserializeByte();
                val1 |= ((ulong)val3 & 0x7F) << val2;
                val2 += 7;

                if ((val3 & 0x80) == 0)
                    if (!signed)
                        return val1;
                    else
                        return (ulong)((long)(val1 >> 1) ^ -(long)(val1 & 1));
            }

            throw new FormatException("Bad7BitEncodedInteger");
        }

        public short DeserializeCompressedInt16() => (short)DeserializeCompressedInt(sizeof(short), signed: true);
        public ushort DeserializeCompressedUInt16() => (ushort)DeserializeCompressedInt(sizeof(ushort), signed: false);
        public int DeserializeCompressedInt32() => (int)DeserializeCompressedInt(sizeof(int), signed: true);
        public uint DeserializeCompressedUInt32() => (uint)DeserializeCompressedInt(sizeof(uint), signed: false);
        public long DeserializeCompressedInt64() => (long)DeserializeCompressedInt(sizeof(long), signed: true);
        public ulong DeserializeCompressedUInt64() => DeserializeCompressedInt(sizeof(ulong), signed: false);

#endregion DeserializeCompressedInt

#region TryDeserializeCompressedInt

        bool TryDeserializeCompressedInt<T>(out T value, int size, bool signed) where T : struct
        {
            var result = false;
            var startPosition = position;

            ulong val1 = 0;
            var val2 = 0;

            var bitVal = size == sizeof(short) ? 21 : size == sizeof(int) ? 35 : size == sizeof(long) ? 63 : 0;

            while (val2 != bitVal)
            {
                if (!TryDeserializeByte(out var val3))
                    break;

                val1 |= ((ulong)val3 & 0x7F) << val2;
                val2 += 7;

                if ((val3 & 0x80) == 0)
                {
                    if (signed)
                        val1 = (ulong)((long)(val1 >> 1) ^ -(long)(val1 & 1));

                    result = true;
                    break;
                }
            }

            if (signed)
                switch (size)
                {
                    case 2: value = (T)(ValueType)(short)val1; break;
                    case 4: value = (T)(ValueType)(int)val1; break;
                    default: value = (T)(ValueType)(long)val1; break;
                }
            else
                switch (size)
                {
                    case 2: value = (T)(ValueType)(ushort)val1; break;
                    case 4: value = (T)(ValueType)(uint)val1; break;
                    default: value = (T)(ValueType)val1; break;
                }

            position = result ? position : startPosition;
            return result;
        }

        public bool TryDeserializeCompressedUInt16(out ushort value) => TryDeserializeCompressedInt(out value, sizeof(ushort), signed: false);
        public bool TryDeserializeCompressedUInt32(out uint value) => TryDeserializeCompressedInt(out value, sizeof(uint), signed: false);
        public bool TryDeserializeCompressedUInt64(out ulong value) => TryDeserializeCompressedInt(out value, sizeof(ulong), signed: false);

        public bool TryDeserializeCompressedInt16(out short value) => TryDeserializeCompressedInt(out value, sizeof(short), signed: true);
        public bool TryDeserializeCompressedInt32(out int value) => TryDeserializeCompressedInt(out value, sizeof(int), signed: true);
        public bool TryDeserializeCompressedInt64(out long value) => TryDeserializeCompressedInt(out value, sizeof(long), signed: true);

#endregion TryDeserializeCompressedInt

#endregion DeserializeNumeric

#region Uri

        public Uri DeserializeUri() => new Uri(DeserializeString());

        public bool TryDeserializeUri(out Uri value, bool throwOnEncodingException = false)
        {
            value = default;

            if (!TryDeserializeString(out var stringValue, throwOnEncodingException))
                return false;

            value = new Uri(stringValue);
            return true;
        }

        #endregion Uri

        #region DeserializeString

        /// <summary>
        /// Deserialize a string from the specified number of bytes
        /// </summary>
        /// <param name="byteCount">The number of bytes to decode</param>
        /// <returns>The deserialized string</returns>
        public string DeserializeString(int byteCount)
        {
            if (byteCount == 0)
                return default;

            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount), "Parameter count must be a positive value");

            EnsureCapacity(byteCount, SerializerOperation.Deserialize);

            position += byteCount;

            return Encoding.GetString(buffer, position - byteCount, byteCount);
        }

        public string DeserializeString()
        {
            if (AutoRaw)
                return DeserializeStringRaw();

            var count = DeserializeInt32();

            return DeserializeString(count);
        }

        public string DeserializeStringRaw()
            => Encoding.GetString(buffer, position, length - position);

        public bool TryDeserializeString(out string value, bool throwOnEncodingException = false)
        {
            value = string.Empty;

            if (!TryDeserializeInt32(out var count))
                return false;

            if (count == 0 || (uint)(position + count) > length)
            {
                position -= Utilities.EncodedInteger.RequiredBytes(count);
                return false;
            }

            try
            {
                value = Encoding.GetString(buffer, position, count);
                position += count;
                return true;
            }
            catch (Exception ex)
            {
                position -= Utilities.EncodedInteger.RequiredBytes(count);

                if (throwOnEncodingException)
                    throw ex;

                return false;
            }
        }

#endregion DeserializeString

#region DeserializeSecureString

        //public SecureString DeserializeSecureString()
        //{
        //    var secureString = (SecureString)null;

        //    var chars = DeserializeChars();

        //    if (chars == null || chars.Length == 0)
        //        return secureString;

        //    unsafe
        //    {
        //        fixed (char* chPtr = chars)
        //            secureString = new SecureString(chPtr, chars.Length);
        //    }

        //    Array.Clear(chars, 0, chars.Length);

        //    return secureString;
        //}

#endregion DeserializeSecureString

#region DeserializeSerializer

        public Serializer DeserializeSerializer()
        {
            if (AutoRaw)
                return ToSerializable();

            var count = DeserializeInt32();

            if (count == 0)
                return default;

            EnsureCapacity(count, SerializerOperation.Deserialize);

#if BRIDGE_NET
            var bytes = DeserializeBytes(count);
            var serializer = new Serializer();
            serializer.Serialize(bytes);
            return serializer;
#else
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    var serializer = new Serializer();
                    serializer.SetLength(count);

                    fixed (byte* src = buffer, dest = serializer.buffer)
                        Utilities.Memory.Memcpy(src + position, dest, count);

                    position += count;
                    return serializer;
                }
            }
#endif
        }

#endregion DeserializeSerializer

#region DeserializeMemoryString

        public MemoryStream DeserializeMemoryStream() => new MemoryStream(DeserializeBytes());

#endregion DeserializeMemoryString

#region DeserializeEnumerable

        public T[] DeserializeArray<T>()
        {
            var count = DeserializeInt32();

            if (count == 0)
                return default;

            var array = new T[count];

            for (var i = 0; i < count; i++)
                array[i] = (T)DeserializeObject(typeof(T));

            return array;
        }

        public IEnumerable<T> DeserializeIEnumerable<T>()
        {
            var count = DeserializeInt32();

            if (count == 0)
                return default;

            var list = new List<T>(capacity: count);

            for (var i = 0; i < count; i++)
                list.Add((T)DeserializeObject(typeof(T)));

            return list;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> DeserializeIEnumerable<TKey, TValue>()
        {
            var count = DeserializeInt32();

            if (count == 0)
                return default;

            var dictionary = new Dictionary<TKey, TValue>(capacity: count);

            for (var i = 0; i < count; i++)
                dictionary.Add((TKey)DeserializeObject(typeof(TKey)), (TValue)DeserializeObject(typeof(TValue)));

            return dictionary;
        }

        public T DeserializeCollection<T>() where T : class, ICollection
        {
            var type = typeof(T);
            var result = DeserializeObject(type);
            return Activator.CreateInstance(type, result) as T;
        }

        public T DeserializeDictionary<T>() where T : class, IDictionary
        {
            var type = typeof(T);
            var result = DeserializeObject(type);
            return Activator.CreateInstance(type, result) as T;
        }

#endregion DeserializeEnumerable

#region Object

        static InvalidOperationException DataException() =>
            new InvalidOperationException(string.Format(Utilities.ResourceStrings.ExceptionFormat, nameof(Serializer),
                nameof(InternalDeserializeObject), Utilities.ResourceStrings.ExceptionMessageBufferDeserializeObject));

        public object TypeDeserializeObject(Type type, bool raw)
        {
            AutoRaw = raw;
            //var @object = GetSerializationMethod(type, SerializerOperation.Deserialize).Invoke(this, null);
            //var obj = GetSerializationMethodFunc(type, SerializerOperation.Serialize)(this);
            var obj = Reflector.Delegate.GetFunc(type)(this);
            AutoRaw = false;

            return obj;
        }

        object InternalDeserializeObject<T>(object value, bool raw, IBackingSerializer serializer = null)
        {
            AutoRaw = false;

            if (serializer != null)
                return serializer.Deserialize<T>(this, rawValue: raw);

            var nullable = default(bool);
            var nullableType = default(Type);
            var type = value?.GetType() ?? typeof(T);

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                nullable = true;
                nullableType = type;
                type = Utilities.Reflection.GetGenericArguments(type)[0];
            }

            if (Reflector.IsKnownType(type))
                return value = TypeDeserializeObject(nullable ? nullableType : type, raw);

            if (nullable)
            {
                var isNotNull = DeserializeBoolean();

                if (!isNotNull)
                    return null;
            }

            var count = 0;

            if (!raw)
            {
                count = DeserializeInt32();

                if (count == 0)
                    return default(T);
                else if (count > 0)
                    EnsureCapacity(count, SerializerOperation.Deserialize);
                else if (CircularReferencesActive)
                { 
                    try { return CircularReferencesIndexDictionary[count * (-1) - 1]; }
                    catch { throw DataException(); }
                }
            }

            value = default(T);
            value = value ?? Activator.CreateInstance(type);

            var firstObject = false;

            //if (CheckCircularReferences)
            //{
            //    if (SerializationList.Count == 0)
            //        firstObject = true;

            //    SerializationList.Add(value);
            //}

            if (CircularReferencesActive)
            {
                if (CircularReferencesIndex == 0)
                    firstObject = true;

                var index = CircularReferencesIndex++;
                //CircularReferencesDictionary.Add(value, index);
                CircularReferencesIndexDictionary.Add(index, value);
            }

            try
            {
                var currentPosition = position;

                if (value is ISerializable)
                    ((ISerializable)value).Deserialize(this);
                else
                {
                    var typeData = default(Reflector.TypeData);
                    var prevTypeData = default(Reflector.TypeData);

                    while (type != typeof(ValueType) && type != typeof(object))
                    {
                        if (typeData == null)
                        {
                            if (!Reflector.TypesCache.TryGetValue(type, out typeData))
                            {
                                typeData = new Reflector.TypeData(type);
                                Reflector.TypesCache.TryAdd(type, typeData);
                            }

                            if (prevTypeData != null)
                                prevTypeData.Parent = typeData;
                        }

                        var deserializedFieldsCount = 0;

                        for (var i = 0; i < typeData.Fields.Length; i++)
                            if (typeData.Fields[i].ShouldSerialize)
                            {
                                var fieldType = typeData.Fields[i].Field.FieldType;
                                var fieldValue = TypeDeserializeObject(fieldType, raw: false);

                                fieldValue = CheckFieldCollectionType(fieldValue, fieldType);

                                typeData.Fields[i].SetValue(value, fieldValue);
                                deserializedFieldsCount++;
                            }

                        if (deserializedFieldsCount == 0)
                            DeserializeByte();

                        prevTypeData = typeData;
                        typeData = typeData.Parent ?? null;
                        type = typeData?.Parent?.Type ?? type.GetTypeInfo().BaseType;
                    }
                }

                if ((!raw && UseObjectSerialization && count != position - currentPosition) || (raw && position != length))
                    throw DataException();

                return value;
            }
            catch (Exception ex) when (!(ex is BufferOverflowException))
            {
                throw DataException();
            }
            finally
            {
                if (firstObject)
                {
                    firstObject = false;
                    CircularReferencesIndex = 0;
                    //CircularReferencesDictionary.Clear();
                    CircularReferencesIndexDictionary.Clear();

                    //if (SerializationList.Capacity > SerializationListBaseCapacity)
                    //    SerializationList.TrimExcess();
                }
            }
        }

        public object DeserializeObject(Type type) => TypeDeserializeObject(type, raw: false);
        public T DeserializeObject<T>() => (T)InternalDeserializeObject<T>(value: null, raw: AutoRaw);
        public T DeserializeObject<T>(T value) where T : class => (T)InternalDeserializeObject<T>(value, raw: false);
        public T DeserializeObject<T>(IBackingSerializer serializer) => (T)InternalDeserializeObject<T>(value: null, raw: false, serializer: serializer);
        public T DeserializeObject<T>(T value, IBackingSerializer serializer) where T : class => (T)InternalDeserializeObject<T>(value, raw: false, serializer: serializer);

        public object DeserializeRawObject(Type type) => TypeDeserializeObject(type, raw: true);
        public T DeserializeRawObject<T>() => (T)InternalDeserializeObject<T>(value: null, raw: true);
        public T DeserializeRawObject<T>(T value) where T : class => (T)InternalDeserializeObject<T>(value, raw: true);
        public T DeserializeRawObject<T>(IBackingSerializer serializer) => (T)InternalDeserializeObject<T>(value: null, raw: true, serializer: serializer);
        public T DeserializeRawObject<T>(T value, IBackingSerializer serializer) where T : class => (T)InternalDeserializeObject<T>(value, raw: true, serializer: serializer);

        #endregion Object

        #region chars

        public char[] DeserializeChars()
        {
            if (AutoRaw)
                return DeserializeCharsRaw();

            var count = DeserializeInt32();

            return DeserializeChars(count);
        }

        public char[] DeserializeCharsRaw()
            => Encoding.GetChars(buffer, position, length - position);

        public char[] DeserializeChars(int byteCount)
        {
            if (byteCount == 0)
                return default;

            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount), "Parameter byteCount must be a positive value");

            EnsureCapacity(byteCount, SerializerOperation.Deserialize);

            position += byteCount;

            return Encoding.GetChars(buffer, position - byteCount, byteCount);
        }

#if !BRIDGE_NET

        public void DeserializeChars(char[] chars, int charIndex, int byteCount)
        {
#if !BRIDGE_NET
            unsafe
            {
                fixed (char* ptr = chars)
                    DeserializeChars(ptr + charIndex, chars.Length - charIndex, byteCount);
            }
#endif
        }

        public unsafe int DeserializeChars(char* chars, int charCount, int byteCount)
        {
            var result = 0;

            if ((IntPtr)chars == IntPtr.Zero)
                throw new ArgumentNullException();

            if (charCount < 0 || byteCount < 0)
                throw new ArgumentOutOfRangeException();

            if (byteCount == 0)
            {
                byteCount = DeserializeInt32();

                if (byteCount == 0)
                    return result;
            }

            EnsureCapacity(byteCount, SerializerOperation.Deserialize);

#if !NETSTANDARD1_0
            fixed (byte* src = buffer)
                result = Encoding.GetChars(src + position, byteCount, chars, charCount);
#else
            var charArray = new char[charCount];
            result = Encoding.GetChars(buffer, position, byteCount, charArray, 0);

            fixed (char* charPtr = charArray)
                Utilities.Memory.Wstrcpy(charPtr, chars, charCount);
#endif

            position += byteCount;

            return result;
        }

        public void DeserializeEncodedChars(int charCount)
            => throw new NotImplementedException();

        //public char[] DeserializeCharsMal(int count)
        //{
        //    //#if BRIDGE_NET
        //    //            return default(char[]);
        //    //#else
        //    if (count < 0)
        //        throw new ArgumentOutOfRangeException("count", "The char count to deserialize cannot be negative");

        //    char[] value;

        //    if (count == 0)
        //    {
        //        count = DeserializeInt32();

        //        if (count == 0)
        //            return default;

        //        EnsureCapacity(count, SerializerOperation.Deserialize);

        //        value = new char[Encoding.GetCharCount(buffer, position, count)];
        //        Encoding.GetChars(buffer, position, count, value, 0);

        //        position += count;
        //        return value;
        //    }

        //    EnsureCapacity(count, SerializerOperation.Deserialize);

        //    var maxByteCount = Encoding.GetMaxByteCount(count);
        //    var byteCount = length - position > maxByteCount ? maxByteCount : length - position;

        //    if (byteCount == 0)
        //        throw new BufferOverflowException();

        //    int charCount;
        //    value = new char[count];

        //    charCount = Encoding.GetChars(buffer, position, byteCount, value, 0);

        //    var realByteCount = Encoding.GetByteCount(value);



        //    //Encoding


        //    //if (charCount != count)
        //    if (realByteCount != count)
        //        throw new InvalidOperationException();

        //    //position += Encoding.GetByteCount(value);
        //    position += realByteCount;
        //    return value;
        //    //#endif
        //}

        //public unsafe void DeserializeChars(char* dest, int destLength, int count)
        //{
        //    if ((IntPtr)dest == IntPtr.Zero)
        //        throw new ArgumentNullException();

        //    if (destLength < 0 || count < 0)
        //        throw new ArgumentOutOfRangeException();

        //    //int charCount;

        //    if (count == 0)
        //    {
        //        count = DeserializeInt32();

        //        if (count == 0)
        //            return;

        //        if (destLength - count < 0)
        //            throw new ArgumentException();

        //        EnsureCapacity(count, SerializerOperation.Deserialize);

        //        fixed (byte* src = buffer)
        //        {
        //            //charCount = Encoding.GetCharCount(src + position, count);
        //            //Encoding.GetChars(src + position, count, dest, charCount);
        //        }

        //        position += count;
        //        return;
        //    }

        //    //Encoding.GetChars()

        //    EnsureCapacity(count * 2, SerializerOperation.Deserialize);

        //    var maxByteCount = Encoding.GetMaxByteCount(count);
        //    var byteCount = length - position > maxByteCount ? maxByteCount : length - position;

        //    if (byteCount == 0)
        //        throw new BufferOverflowException();

        //    //fixed (byte* src = Buffer)
        //    //   charCount = Encoding.GetChars(src + position, byteCount, dest, count);

        //    //if (charCount != count)
        //    //    throw new InvalidOperationException();

        //    //position += Encoding.GetByteCount(dest, count);
        //}

#endif

        #endregion chars

        #region bytes

        public byte[] DeserializeBytes() => AutoRaw ? DeserializeBytesRaw() : DeserializeBytes(0);

        public byte[] DeserializeBytesRaw() => DeserializeBytes(length - position);

        public byte[] DeserializeBytes(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "The byte count to deserialize cannot be negative");

            if (count == 0)
                count = DeserializeInt32();

            if (count == 0)
                return default;

            EnsureCapacity(count, SerializerOperation.Deserialize);

            var value = new byte[count];

#if !BRIDGE_NET
            unsafe
            {
                fixed (byte* src = buffer, dest = value)
                    Utilities.Memory.Memcpy(src + position, dest, count);
            }
#else
            Utilities.Memory.BlockCopy(buffer, position, value, 0, count);
#endif

            position += count;
            return value;
        }

#if !BRIDGE_NET

        public unsafe void DeserializeBytes(byte[] dest, int index, int count)
        {
            fixed (byte* ptr = dest)
                DeserializeBytes(ptr + index, dest.Length, count);
        }

        public unsafe void DeserializeBytes(byte* destination, int destinationSize, int bytesToCopy)
        {
            if ((IntPtr)destination == IntPtr.Zero)
                throw new ArgumentNullException(nameof(destination));

            if (destinationSize < 0)
                throw new ArgumentOutOfRangeException(nameof(destinationSize), $"{destinationSize} must be a positive value");

            if (bytesToCopy < 0)
                throw new ArgumentOutOfRangeException(nameof(bytesToCopy), $"{bytesToCopy} must be a positive value");

            if (bytesToCopy == 0)
                bytesToCopy = DeserializeInt32();

            if (bytesToCopy == 0)
                return;

            if (destinationSize - bytesToCopy < 0)
                throw new ArgumentOutOfRangeException(nameof(bytesToCopy), $"{nameof(bytesToCopy)} is greater than {nameof(destinationSize)}");

            EnsureCapacity(bytesToCopy, SerializerOperation.Deserialize);

            fixed (byte* src = buffer)
                Utilities.Memory.Memcpy(src + position, destination, bytesToCopy);

            position += bytesToCopy;
        }

#endif

#endregion bytes

#endregion Deserialize
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
