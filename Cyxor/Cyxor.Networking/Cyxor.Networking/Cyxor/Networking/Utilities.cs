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
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

#if !NET35
    using System.Net.Security;
#endif

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cyxor.Networking
{
#if NET35
    using Config;
#endif

    using Models;
    using Extensions;
    using Serialization;
    using System.Threading.Tasks;

    public class Utilities : Serialization.Utilities
    {
        protected Utilities()
        {

        }

        public const int BaseMTUSize = 1416;

        //public static class SerializerUtils
        //{
        //    public static Serialization.Serializer FromFile(string path)
        //        => new Serialization.Serializer(File.ReadAllBytes(path));
        //}

        public static class Http
        {
            public const string NewLine = "\r\n";
            public const string HttpHeaderEnd = NewLine + NewLine;
            public static readonly char[] HttpHeaderEndChars = HttpHeaderEnd.ToCharArray();
        }

        public static class Json
        {
            // TODO: Try to implement a concurrent cache
            class ShouldSerializeContractResolver : DefaultContractResolver
            {
                readonly object Target;
                readonly Type TargetType;
                readonly IEnumerable<string> Includes;

                public ShouldSerializeContractResolver(object target, IEnumerable<string> includes)
                {
                    Target = target;
                    Includes = includes;
                    TargetType = target.GetType();
                }

                string GetPath(MemberInfo member)
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append(member.Name);

                    var type = member.DeclaringType;

                    while (type != null && type != TargetType && type != typeof(object) && type != typeof(ValueType))
                    {
                        stringBuilder.Insert(0, '.');
                        stringBuilder.Insert(0, type.Name);
                        type = type.GetTypeInfo().BaseType;
                    }

                    return stringBuilder.ToString();
                }

                protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                {
                    var property = base.CreateProperty(member, memberSerialization);

                    property.ShouldSerialize = p =>
                    {
                        var jsonContract = ResolveContract(property.PropertyType);
                        var contractType = typeof(JsonContract).GetAnyDeclaredField("ContractType").GetValue(jsonContract);

                        if (contractType.ToString() == nameof(Object))
                        {
                            if (Includes != null)
                                if (Includes.Any(q => q.StartsWith(GetPath(member))))
                                    return true;

                            return false;
                        }

                        return true;
                    };

                    //var jsonContract = ResolveContract(property.PropertyType);
                    //var contractType = typeof(JsonContract).GetAnyDeclaredField("ContractType").GetValue(jsonContract);

                    //if (contractType.ToString() == nameof(Object))
                    //{
                    //    var shouldSerialize = false;

                    //    if (Includes != null)
                    //        if (Includes.Any(p => p.StartsWith(GetPath(member))))
                    //            shouldSerialize = true;

                    //    property.ShouldSerialize = p => shouldSerialize;
                    //}

                    return property;
                }
            }

            class JsonCommentSerializer
            {
                class JsonCommentWriter : JsonTextWriter
                {
                    object Target;
                    bool NeedReorderLastComment;
                    readonly StringWriter StringWriter;
                    //readonly IEnumerable<string> Includes;

                    public override string ToString() => StringWriter?.GetStringBuilder().ToString();

                    public JsonCommentWriter(StringWriter stringWriter, object target)//, IEnumerable<string> includes = null)
                        : base(stringWriter)
                    {
                        Target = target;
                        //Includes = includes;
                        StringWriter = stringWriter;
                    }

                    //bool IsRootPath() => !Path.Contains('.');

                    public override void WritePropertyName(string name)
                    {
                        base.WritePropertyName(name);
                        WriteComment();
                    }

                    public override void WritePropertyName(string name, bool escape)
                    {
                        base.WritePropertyName(name, escape);
                        WriteComment();
                    }
#if !NET35 && !NET40
                    public override async System.Threading.Tasks.Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = default)
                    {
                        await base.WritePropertyNameAsync(name, cancellationToken);
                        WriteComment();
                    }

                    public override async System.Threading.Tasks.Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = default)
                    {
                        await base.WritePropertyNameAsync(name, escape, cancellationToken);
                        WriteComment();
                    }
#endif
                    public void ReorderLastComment()
                    {
                        if (!NeedReorderLastComment)
                            return;

                        NeedReorderLastComment = false;

                        var sb = StringWriter.GetStringBuilder();
                        var text = sb.ToString();

                        var index1 = text.LastIndexOf("/*");
                        var index2 = text.LastIndexOf("*/") + 3;

                        if (index1 != -1 && text[index2] != '{' && text[index2] != '[')
                        {
                            var index3 = text.IndexOf(Environment.NewLine, index2);

                            var commentToken = text.Substring(index1, index2 - index1 - 1);
                            var valueToken = text.Substring(index2, index3 - index2);

                            sb.Replace(valueToken, commentToken, index2, valueToken.Length);
                            sb.Replace(commentToken, valueToken, index1, commentToken.Length);
                        }
                    }

                    public void WriteComment()
                    {
                        ReorderLastComment();

                        var target = Target;
                        var type = Target.GetType();
                        var memberInfo = default(MemberInfo);
                        var propertyNames = Path.Split('.');

                        for (var i = 0; i < propertyNames.Length; i++)
                        {
                            memberInfo = type.GetProperties().FirstOrDefault(p => p.Name == propertyNames[i]);

                            if (memberInfo == null)
                                memberInfo = type.GetFields().FirstOrDefault(p => p.Name == propertyNames[i]);

                            if (memberInfo is PropertyInfo propertyInfo)
                            {
                                target = propertyInfo.GetGetMethod().Invoke(target, parameters: null);
                                type = target?.GetType() ?? propertyInfo.PropertyType;
                            }
                            else if (memberInfo is FieldInfo fieldInfo)
                            {
                                target = fieldInfo.GetValue(target);
                                type = target?.GetType() ?? fieldInfo.FieldType;
                            }
                        }

                        var attributes = memberInfo?.GetCustomAttributes(inherit: true);
                        var attribute = attributes?.SingleOrDefault(p => p.GetType() == typeof(DescriptionAttribute));

                        var classDescription = type.GetTypeInfo()
                            .GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;

                        var memberDescription = (attribute as DescriptionAttribute)?.Description;

                        var description = classDescription != null && memberDescription != null ?
                            $"{classDescription}. {memberDescription}" : classDescription ?? memberDescription;

                        //var description = (attribute as DescriptionAttribute)?.Description ?? type.GetTypeInfo()
                        //    .GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;

                        if (description != null)
                        {
                            WriteComment(description);
                            NeedReorderLastComment = true;
                        }
                    }
                }

                JsonSerializer Serializer { get; }

                //public static JsonCommentSerializer Instance { get; } = new JsonCommentSerializer();

                public JsonCommentSerializer()
                {
                    Serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        //PreserveReferencesHandling = PreserveReferencesHandling.All,
                    };

                    Serializer.Converters.Add(new BinaryConverter());
                    Serializer.Converters.Add(new StringEnumConverter());
                }

                public virtual string Serialize(object target, bool excludeDefaultValues = default, bool excludeComments = default, IEnumerable<string> includes = default)
                {
                    if (excludeDefaultValues)
                        Serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
                    else
                        Serializer.DefaultValueHandling = DefaultValueHandling.Include;

                    if (includes != null)
                        Serializer.ContractResolver = new ShouldSerializeContractResolver(target, includes);

                    var stringWriter = new StringWriter();
                    var jsonWriter = default(JsonTextWriter);

                    if (excludeComments)
                        jsonWriter = new JsonTextWriter(stringWriter);
                    else
                        jsonWriter = new JsonCommentWriter(stringWriter, target);

                    Serializer.Serialize(jsonWriter, target);

                    if (!excludeComments)
                        (jsonWriter as JsonCommentWriter).ReorderLastComment();

                    //return jsonWriter.ToString();
                    return stringWriter.GetStringBuilder().ToString();
                }

                public object Deserialize(string value)
                {
                    var settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
                    return JsonConvert.DeserializeObject(value, settings);
                }

                public T Deserialize<T>(string value)
                {
                    var settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
                    return JsonConvert.DeserializeObject<T>(value, settings);
                }

                public object Deserialize(string value, Type type)
                {
                    var settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
                    return JsonConvert.DeserializeObject(value, type, settings);
                }

                public void Populate(string value, object target)
                {
                    var settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
                    JsonConvert.PopulateObject(value, target, settings);
                }
            }

            public static string SerializeIncludes(object target, bool includeComments = false, bool excludeDefaultValues = false, IEnumerable<string> includes = default)
                => new JsonCommentSerializer().Serialize(target, excludeDefaultValues, excludeComments: !includeComments, includes);

            public static string Serialize(object target, bool includeComments = false, bool excludeDefaultValues = false, IEnumerable<string> includes = default)
                => includeComments ? new JsonCommentSerializer().Serialize(target, excludeDefaultValues, excludeComments: !includeComments, includes) : JsonConvert.SerializeObject(target,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        //PreserveReferencesHandling = PreserveReferencesHandling.All,
                        DefaultValueHandling = excludeDefaultValues ? DefaultValueHandling.Ignore : DefaultValueHandling.Include
                    });

            public static object Deserialize(string value) => new JsonCommentSerializer().Deserialize(value);
            public static T Deserialize<T>(string value) => new JsonCommentSerializer().Deserialize<T>(value);
            public static object Deserialize(string value, Type type) => new JsonCommentSerializer().Deserialize(value, type);
            public static void Populate(string value, object target) => new JsonCommentSerializer().Populate(value, target);
        }

        public static class Platform
        {
            public enum PlatformOS
            {
                Linux,
                MacOSX,
                Windows,
                Unknown,
            }

            public static string NETRuntime => IsMonoRuntime ? "Mono" : "Microsoft";
            public static bool IsMonoRuntime => Type.GetType("Mono.Runtime") != null;

            public static PlatformOS OS
            {
                get
                {
#if NET35 || NET40
                    if (Environment.OSVersion.Platform == PlatformID.MacOSX)
                        return PlatformOS.MacOSX;
                    else if (Environment.OSVersion.Platform == PlatformID.Unix)
                        return PlatformOS.Linux;
                    else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                        return PlatformOS.Windows;

                    return PlatformOS.Unknown;
#else
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        return PlatformOS.MacOSX;
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        return PlatformOS.Linux;
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        return PlatformOS.Windows;

                    return PlatformOS.Unknown;
#endif
                }
            }
        }

        public static class Models
        {
            public static Result Validate(Node node, object instance)
            {
                var validationErrors = new List<ValidationError>();

                var validations = (instance as IValidatable)?.Validate(node);

                if (validations != null)
                    validationErrors.AddRange(validations);

#if !NET35 && !NET40
                var validationResults = new List<ValidationResult>();

                Validator.TryValidateObject(instance, new ValidationContext(instance), validationResults, true);

                foreach (var validationResult in validationResults)
                    validationErrors.Add(new ValidationError { ErrorMessage = validationResult.ErrorMessage, MemberNames = validationResult.MemberNames });
#endif

                if (validationErrors.Count > 0)
                {
                    //var validationApiModel = new ValidationApiModel { Errors = validationErrors };
                    //return new Result(ResultCode.Error, "Validation Errors", model: new Serialization.Serializer(validationApiModel, new JsonBackingSerializer()));
                    return new Result(ResultCode.Error, "Validation Errors", model: validationErrors);
                }

                return Result.Success;
            }
        }

        internal static class Dns
        {
            internal static System.Threading.Tasks.Task<IPHostEntry> GetHostEntryAsync(string hostNameOrAddress) =>
#if NET35 || NET40
                //Task.Run<IPHostEntry>(() => System.Net.Dns.GetHostEntry(hostNameOrAddress));
                Task.Run(() => System.Net.Dns.GetHostEntry(hostNameOrAddress));
//#elif NET40
//              System.Net.DnsEx.GetHostEntryAsync(hostNameOrAddress);
#else
                System.Net.Dns.GetHostEntryAsync(hostNameOrAddress);
#endif

            internal static System.Threading.Tasks.Task<IPAddress[]> GetHostAddressesAsync(string hostNameOrAddress) =>
#if NET35 || NET40
                //Task.Run<IPAddress[]>(() => System.Net.Dns.GetHostAddresses(hostNameOrAddress));
                Task.Run(() => System.Net.Dns.GetHostAddresses(hostNameOrAddress));
//#elif NET40
//              System.Net.DnsEx.GetHostAddressesAsync(hostNameOrAddress);
#else
                System.Net.Dns.GetHostAddressesAsync(hostNameOrAddress);
#endif
        }

        internal static class Socket
        {
            internal static bool GetDualMode(System.Net.Sockets.Socket socket)
#if NET35
                => false;
#else
            {
                if (System.Net.Sockets.Socket.OSSupportsIPv6)
                    return socket.GetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, System.Net.Sockets.SocketOptionName.IPv6Only, 4)[0] == 0;
                return false;
            }
#endif

            internal static void SetDualMode(System.Net.Sockets.Socket socket, bool value)
            {
#if !(NET35)
                if (System.Net.Sockets.Socket.OSSupportsIPv6)
                    socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.IPv6, System.Net.Sockets.SocketOptionName.IPv6Only, !value);
#endif
            }
        }

        internal static class SslStream
        {
            internal static System.Net.Security.SslStream Create(
                System.IO.Stream innerStream, bool leaveInnerStreamOpen,
                System.Net.Security.RemoteCertificateValidationCallback userCertificateValidationCallback,
                System.Net.Security.LocalCertificateSelectionCallback userCertificateSelectionCallback, EncryptionPolicy encryptionPolicy) =>
#if NET35
                new System.Net.Security.SslStream(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback);
#else
                new System.Net.Security.SslStream(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback, encryptionPolicy);
#endif
        }

        public static class Task
        {
            /// <summary>Gets a task that's already been completed successfully.</summary>
            public static System.Threading.Tasks.Task CompletedTask { get; }
#if NET45 || NET451 || NET40 || NET35
                = FromResult<object>(null);
#else
                = System.Threading.Tasks.Task.CompletedTask;
#endif

            public static System.Threading.Tasks.Task Delay(int millisecondsDelay)
#if NET40 || NET35
                => System.Threading.Tasks.TaskEx.Delay(millisecondsDelay);
#else
                => System.Threading.Tasks.Task.Delay(millisecondsDelay);
#endif

            public static System.Threading.Tasks.Task Delay(TimeSpan delay) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Delay(delay);
#else
            System.Threading.Tasks.Task.Delay(delay);
#endif

            public static System.Threading.Tasks.Task Delay(int millisecondsDelay, System.Threading.CancellationToken cancellationToken) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Delay(millisecondsDelay, cancellationToken);
#else
            System.Threading.Tasks.Task.Delay(millisecondsDelay, cancellationToken);
#endif

            public static System.Threading.Tasks.Task Delay(TimeSpan delay, System.Threading.CancellationToken cancellationToken) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Delay(delay, cancellationToken);
#else
            System.Threading.Tasks.Task.Delay(delay, cancellationToken);
#endif

            public static System.Threading.Tasks.Task<TResult> FromResult<TResult>(TResult result) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.FromResult(result);
#else
            System.Threading.Tasks.Task.FromResult(result);
#endif

            public static System.Threading.Tasks.Task Run(Action action) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(action);
#else
            System.Threading.Tasks.Task.Run(action);
#endif

            public static System.Threading.Tasks.Task Run(Func<System.Threading.Tasks.Task> function) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(function);
#else
            System.Threading.Tasks.Task.Run(function);
#endif

            public static System.Threading.Tasks.Task<TResult> Run<TResult>(Func<TResult> function) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(function);
#else
            System.Threading.Tasks.Task.Run(function);
#endif

            public static System.Threading.Tasks.Task<TResult> Run<TResult>(Func<System.Threading.Tasks.Task<TResult>> function) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(function);
#else
            System.Threading.Tasks.Task.Run(function);
#endif

            public static System.Threading.Tasks.Task Run(Action action, System.Threading.CancellationToken cancellationToken) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(action, cancellationToken);
#else
            System.Threading.Tasks.Task.Run(action, cancellationToken);
#endif

            public static System.Threading.Tasks.Task<TResult> Run<TResult>(Func<System.Threading.Tasks.Task<TResult>> function, System.Threading.CancellationToken cancellationToken) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(function, cancellationToken);
#else
            System.Threading.Tasks.Task.Run(function, cancellationToken);
#endif

            public static System.Threading.Tasks.Task Run(Func<System.Threading.Tasks.Task> function, System.Threading.CancellationToken cancellationToken) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(function, cancellationToken);
#else
            System.Threading.Tasks.Task.Run(function, cancellationToken);
#endif

            static System.Threading.Tasks.Task<TResult> Run<TResult>(Func<TResult> function, System.Threading.CancellationToken cancellationToken) =>
#if NET40 || NET35
            System.Threading.Tasks.TaskEx.Run(function, cancellationToken);
#else
            System.Threading.Tasks.Task.Run(function, cancellationToken);
#endif

#if NET35 || NET40
            public static System.Runtime.CompilerServices.YieldAwaitable Yield() => System.Threading.Tasks.TaskEx.Yield();
//#elif NET40
//            public static Microsoft.Runtime.CompilerServices.YieldAwaitable Yield() => System.Threading.Tasks.TaskEx.Yield();
#else
            public static System.Runtime.CompilerServices.YieldAwaitable Yield() => System.Threading.Tasks.Task.Yield();
#endif
        }

        public static class Threading
        {
            public interface IReferenceCounted
            {
                int Acquire();
                int Release();

                Reference Reference { get; }
            }

            public struct Reference : IDisposable
            {
                public IReferenceCounted ReferenceCounted { get; set; }

                public Reference(IReferenceCounted referenceCounted)
                {
                    ReferenceCounted = referenceCounted;
                    ReferenceCounted?.Acquire();
                }

                public void Dispose() => ReferenceCounted?.Release();
            }

            public struct InterlockedInt
            {
                int location;

                public int Value
                {
                    get => location;
                    set => location = value;
                }

                public int Increment() => Interlocked.Increment(ref location);

                public int Decrement() => Interlocked.Decrement(ref location);

                public int Add(int value) => Interlocked.Add(ref location, value);

                public int Exchange(int value) => Interlocked.Exchange(ref location, value);

                public int CompareExchange(int value, int comparand) => Interlocked.CompareExchange(ref location, value, comparand);

                public override string ToString() => $"[{nameof(Value)} = {Value}]";
            }

            public struct InterlockedLong
            {
                long location;

                public long Value
                {
                    get => IntPtr.Size == 8 ? location : Interlocked.Read(ref location);
                    set => location = IntPtr.Size == 8 ? value : Interlocked.Exchange(ref location, value);
                }

                /// <summary>
                /// Increments the <see cref="Value"/> and stores the result, as an atomic operation.
                /// </summary>
                /// <returns>The incremented value.</returns>
                public long Increment() => Interlocked.Increment(ref location);

                /// <summary>
                /// Decrements the <see cref="Value"/> and stores the result, as an atomic operation.
                /// </summary>
                /// <returns>The decremented value.</returns>
                public long Decrement() => Interlocked.Decrement(ref location);

                public long Add(long value) => Interlocked.Add(ref location, value);

                public long Exchange(long value) => Interlocked.Exchange(ref location, value);

                public long CompareExchange(long value, long comparand) => Interlocked.CompareExchange(ref location, value, comparand);

                public override string ToString() => $"[{nameof(Value)} = {Value}]";
            }

            public struct InterlockedCountdown
            {
                volatile int count;

                public int Count
                {
                    get
                    {
                        var observedCount = count;
                        return observedCount > 0 ? observedCount : 0;
                    }
                }

                public int InitialCount { get; private set; }

                public bool IsCompleted => count <= 0;

                public InterlockedCountdown(int initialCount)
                {
                    if (initialCount < 0)
                        throw new ArgumentOutOfRangeException(nameof(initialCount));

                    count = initialCount;
                    InitialCount = initialCount;
                }

                public int Acquire()
                {
                    var observedCount = 0;
                    var spinner = new SpinWait();

                    while (true)
                    {
                        observedCount = count;

                        if (observedCount <= 0)
                            return observedCount;
                        else if (observedCount == int.MaxValue)
                            return int.MaxValue;

#pragma warning disable 0420
                        if (Interlocked.CompareExchange(ref count, observedCount + 1, observedCount) == observedCount)
                            break;
#pragma warning restore 0420

                        spinner.SpinOnce();
                    }

                    return observedCount + 1;
                }

                public bool TryAcquire()
                {
                    var observedCount = 0;
                    var spinner = new SpinWait();

                    while (true)
                    {
                        observedCount = count;

                        if (observedCount <= 0)
                            return false;
                        else if (observedCount == int.MaxValue)
                            throw new InvalidOperationException($"{nameof(InterlockedCountdown)} max value reached.");

#pragma warning disable 0420
                        if (Interlocked.CompareExchange(ref count, observedCount + 1, observedCount) == observedCount)
                            break;
#pragma warning restore 0420

                        spinner.SpinOnce();
                    }

                    return true;
                }

                public bool TryRelease()
                {
                    if (count <= 0)
                        throw new InvalidOperationException($"{nameof(InterlockedCountdown)} decrement below zero.");

#pragma warning disable 0420
                    var newCount = Interlocked.Decrement(ref count);
#pragma warning restore 0420

                    if (newCount == 0)
                        return true;

                    if (newCount < 0)
                        throw new InvalidOperationException($"{nameof(InterlockedCountdown)} decrement below zero.");

                    return false;
                }

                public int Release()
                {
                    var newCount = count;

                    if (newCount <= 0)
                        return newCount;

#pragma warning disable 0420
                    newCount = Interlocked.Decrement(ref count);
#pragma warning restore 0420

                    return newCount;
                }

                public void Reset(int initialCount)
                {
                    if (initialCount < 0)
                        throw new ArgumentOutOfRangeException(nameof(initialCount));

                    count = initialCount;
                    InitialCount = initialCount;
                }

                public void Reset() => Reset(InitialCount);

                public override string ToString() => $"[{nameof(Count)} = {Count}] [{nameof(InitialCount)} = {InitialCount}] [{nameof(IsCompleted)} = {IsCompleted}]";
            }

            //public interface IAwaiter : INotifyCompletion
            //{
            //    Result GetResult();
            //    bool IsCompleted { get; }
            //    Awaitable GetAwaiter();
            //    Awaitable ConfigureAwait(bool continueOnCapturedContext);
            //}

            public interface IAwaiter
            {
                Awaitable GetAwaiter();
                Awaitable ConfigureAwait(bool continueOnCapturedContext);
            }

            // TODO: Complete
            public struct AsyncAwaiterMethodBuilder<TResult>
            {
                static readonly AsyncAwaiterMethodBuilder<TResult> Default = new AsyncAwaiterMethodBuilder<TResult>();

                //public AwaitableValue<TResult> Task { get; }
                public Awaitable Task { get; }

                public static AsyncAwaiterMethodBuilder<TResult> Create() => Default;

                public void SetException(Exception exception) { }

                public void SetResult(TResult result) { }

                public void SetStateMachine(IAsyncStateMachine stateMachine) { }

                public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine { }

                public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                    where TAwaiter : INotifyCompletion
                    where TStateMachine : IAsyncStateMachine { }

                [System.Security.SecuritySafeCritical]
                public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                    where TAwaiter : ICriticalNotifyCompletion
                    where TStateMachine : IAsyncStateMachine { }
            }

            // TODO: Convert Awaitable to a SomeTask<TResult>
//#if !NETSTANDARD2_0
//            [AsyncMethodBuilder(typeof(AsyncAwaiterMethodBuilder<>))]
//#endif
            public class Awaitable : IAwaiter, INotifyCompletion
            {
                public static Awaitable CompletedAwaitable = new Awaitable { Completed = new InterlockedInt { Value = 1 } };

                static Action Fake { get; } = () => { };

                Result Result;

                Action Continuation;

                InterlockedInt Completed;

                Action action;
                public Action Action
                {
                    get => action;
                    set => action = this != CompletedAwaitable ? value : null;
                }

                public Result GetResult() => Result;

                public bool IsCompleted => Completed.Value == 1;

                public Awaitable GetAwaiter()
                {
                    Action?.Invoke();
                    return this;
                }

                /// <summary>
                /// This method doesn't use any context, it exists only for an homogeneous implementation with Tasks.
                /// </summary>
                public Awaitable ConfigureAwait(bool continueOnCapturedContext) => this;

                public bool TrySetResult(Result result)
                {
                    if (Completed.CompareExchange(1, 0) != 0)
                        return false;

                    Result = result;
                    (Continuation ?? Interlocked.CompareExchange(ref Continuation, Fake, null))?.Invoke();
                    return true;
                }

                void INotifyCompletion.OnCompleted(Action continuation)
                {
                    if (Continuation == Fake || Interlocked.CompareExchange(ref Continuation, continuation, null) == Fake)
                        Task.Run(continuation);
                }

                public virtual void Reset()
                {
                    if (this == CompletedAwaitable)
                        return;

                    Completed.Value = 0;
                    Continuation = null;
                }

                public override string ToString() => $"[{nameof(IsCompleted)} = {IsCompleted}] [{nameof(Continuation)} = {Continuation != null}]";
            }
        }

        internal static class ResourceStrings
        {
            //internal const string ClientAlreadyConnected = "Client already connected.";
            //internal const string IncorrectLoginParameters = "Incorrect login parameters.";
            //internal const string NetworkUnavailable = "Network unavailable.";
            //internal const string ConnectionInProgress = "There is a connection in progress.";
            //internal const string DisconnectionInProgress = "There is a disconnection in progress.";

            // Server
            public const string

            CyxorInternalException = "Cyxor internal error. You can report this as a bug.",
            AggregateException = "Review the inner exceptions for details.",

            ProtocolError = "Mal formed packet.",
            CryptographicError = "Unable to decrypt received data.",

            NameTaken = "A client with that name is already connected.",
            //internal const string ConnectionDenied = "The server denied the connection.";

            SRPAuthFailed = "SRP authentication failed.",
            InvalidOperation = "Invalid operation <{0}> for the current connection state.",
            InvalidInternalOperation = "Invalid internal operation <{0}> for the current connection state.",



            InvalidClientName = "The client name is invalid.",
            ClientConnectTimeout = "Timeout waiting for server response.",
            InvalidSynchronousThread = "Invalid thread for synchronous events",

            // New
            LogNodeInterlocked = "Invalid state for interlocked connection.",

            ClientDebuggerDisplayFormat = "[Name = {0}]  [State = {1}]  [Address = {2}:{3}]",
            ServerDebuggerDisplayFormat = "[Connected = {0}] [Connections = {1}] [Address = {2}:{3}]",

            ExceptionNodeDisposed = "Node disposed.",

            ExceptionFormat = "Cyxor..{0}.{1}() : {2}",
            ExceptionFormat1 = "Cyxor..{0}.{1}({2}) : {3}",
            ExceptionFormat2 = "Cyxor..{0}.{1}({2}, {3}) : {4}",
            ExceptionFormat3 = "Cyxor..{0}.{1}({2}, {3}, {4}) : {5}",
            ExceptionMessageBufferReadNumeric = "",
            ExceptionMessageBufferReadObject = "Deserialization operation do not match number of bytes written in the Serialization process.";
        }

        public static bool IsValidEmailFormat(string email)
        {
            if (email == null)
                throw new ArgumentNullException();

            return Regex.IsMatch(email, @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        }

        public static IPAddress TryParseAddress(string address)
        {
            var ipAddress = (IPAddress)null;

            if (IPAddress.TryParse(address, out ipAddress))
                return ipAddress;

            //try
            //{
            //    // TODO: DnsEx ??
            //    IPHostEntry host = Dns.GetHostEntry(address);

            //    foreach (IPAddress ip in host.AddressList)
            //        if (System.Net.Sockets.Socket.OSSupportsIPv6 && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            //            return ip;
            //        else if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //            return ip;
            //}
            //catch { }

            return null;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
