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
using System.Threading;
using System.Reflection;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

#if NET40
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
#elif !NET35
using System.Composition;
using System.Composition.Hosting;

using System.Composition.Hosting.Core;
using System.Composition.Convention;
#endif

using AgileObjects.AgileMapper;

//#if !NET35
//using AutoMapper;
//#endif

namespace Cyxor.Networking
{
    using Config;
    using Extensions;
    using Controllers;

    using static Utilities.Threading;

//#if !NET35
//    [Export(typeof(IPlugin))]
//    public class EmailSender1 : IPlugin
//    {
//        public Assembly Assembly => throw new NotImplementedException();

//        public void Initialize(Node node) => throw new NotImplementedException();
//    }

//    [Export(typeof(IPlugin))]
//    public class EmailSender2 : IPlugin
//    {
//        public Assembly Assembly => throw new NotImplementedException();

//        public void Initialize(Node node) => throw new NotImplementedException();
//    }
//#endif

//    public class Klain
//    {
//#if !NET35
//        [ImportMany(nameof(IPlugin))]
//        //[ImportMany]
//        public IEnumerable<IPlugin> Plugink { get; set; }
//        //public IPlugin[] Plugins { get; set; }

//        //[Import]
//        //public IPlugin Plugink { get; set; }
//#endif
//    }

    public abstract partial class Node : Awaitable, IDisposable
    {
        internal abstract void Internal();

        //Mutex Mutex;
        internal bool AppMode { get; set; }
        internal bool WaitingForCommand { get; set; }

        public static Node Instance { get; }
        public static bool IsUnityEditor { get; set; }

        internal Socket UdpSocket;
        internal virtual Socket TcpSocket { get; set; }

        internal IPHostEntry IPHostEntry { get; set; }
        internal protected EndPoint LocalEndPoint { get; set; }

        internal string DisconnectReason;

        internal NodePools Pools { get; }
        HashSet<Assembly> LoadedPluginAssemblies { get; }
        protected internal NodeMiddleware Middleware { get; set; }
        internal ConcurrentQueue<Events.ActionEventArgs> EventQueue { get; }
        internal ConcurrentDictionary<Type, Controller> ControllerInstances { get; }

        protected bool Disposed { get; private set; }

        public Client AsClient { get; }
        public Server AsServer { get; }
        public bool IsClient => AsClient != null;
        public bool IsServer => AsServer != null;
        public string CoreTypeName => IsClient ? nameof(Client) : nameof(Server);

#if !NET35
        [ImportMany]
        public IEnumerable<IPlugin> Plugins { get; set; }
#endif

        public NodeLinkage Linkage { get; }
        public NodeEvents Events { get; set; }
        public abstract bool IsConnected { get; }
        public NodeStatistics Statistics { get; internal set; }
        public NodeControllers Controllers { get; protected set; }
        public NodeNetworkInformation NetworkInformation { get; internal set; }

        protected virtual IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Scope used during a local API call <see cref="NodeControllers.ExecuteAsync(string)"/>
        /// </summary>
        protected internal virtual IServiceScope CommandScope { get; set; }

        /// <summary>
        /// Scope used during <see cref="Node"/> <see cref="ConnectAsync"/> and <see cref="DisconnectAsync"/>
        /// </summary>
        public virtual IServiceScope ConnectionScope { get; set; }

        public Context Context { get; set; }

//#if !NET35
//        public AutoMapper.IMapper Mapper { get; set; }
//#endif

        public IMapper Mapper { get; set; }

        NodeConfig config;
        public NodeConfig Config
        {
            get => config;
            set
            {
                if (IsConnected)
                    throw new InvalidOperationException("The network Node must be disconnected to perform this operation.");

                config = value;
                config.Node = this;
            }
        }

        public Node()
        {
            AsClient = this as Client;
            AsServer = this as Server;

            Pools = new NodePools(this);
            TrySetResult(Result.Success);
            Linkage = new NodeLinkage(this);
            //Controllers = new NodeControllers(this);
            LoadedPluginAssemblies = new HashSet<Assembly>();
            EventQueue = new ConcurrentQueue<Events.ActionEventArgs>();

            ControllerInstances = new ConcurrentDictionary<Type, Controller>();

            Mapper = AgileObjects.AgileMapper.Mapper.CreateNew();

            Mapper.WhenMapping.MapNullCollectionsToNull();

//#if !NET35
//            Mapper = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(p => p.ForAllMaps((map, expr) =>
//            {
//                foreach (var propName in map.GetUnmappedPropertyNames())
//                {
//                    var srcPropInfo = map.SourceType.GetAnyDeclaredProperty(propName);

//                    if (srcPropInfo != null)
//                        expr.ForSourceMember(propName, opt => opt.Ignore());

//                    var destPropInfo = map.DestinationType.GetAnyDeclaredProperty(propName);

//                    if (destPropInfo != null)
//                        expr.ForMember(propName, opt => opt.Ignore());
//                }
//            })));
//#endif
        }

        void IDisposable.Dispose()
        {
            if (!Disposed)
            {
                DisconnectAsync(new Result(comment: Utilities.ResourceStrings.ExceptionNodeDisposed)).Wait();

                IPHostEntry = null;

                TcpSocket.Dispose();
                UdpSocket.Dispose();

                Disposed = true;
            }
        }

        public virtual IServiceScope CreateScope(bool resetServiceProvider = false) => default;

        //public virtual object GetService(Type serviceType, bool allowSubclasses = true) => null;

        public virtual Connection CreateConnection() => new Connection();
        internal Connection CreateConnection(Link link) => new Connection(link);

        //public Events.MessageLoggedEventArgs Log(Result result)
        //{
        //    var logCategory = LogCategory.Success;

        //    logCategory = result.Exception != null ? LogCategory.Fatal : result.Code != ResultCode.Success ? LogCategory.Error : LogCategory.Success;

        //    if (result.Exception != null)
        //        logCategory = LogCategory.Fatal;
        //    else if (result.Code != ResultCode.Success)
        //        logCategory = LogCategory.Error;

        //    return Log(logCategory, result.Exception, result.ToString());
        //}

        public Events.MessageLoggedEventArgs Log(Result result) =>
            Log(result, isCommandResult: false);
        public Events.MessageLoggedEventArgs Log(Result result, bool isCommandResult) =>
            Events.Post(new Events.MessageLoggedEventArgs(this, result, isCommandResult));
        public Events.MessageLoggedEventArgs Log(string message, params object[] args) =>
            Log(LogCategory.Message, 0, default, message, args);
        public Events.MessageLoggedEventArgs Log(LogCategory category, Exception exception) =>
            Log(category, 0, exception, message: null, args: null);
        public Events.MessageLoggedEventArgs Log(LogCategory category, int indentLevel, Exception exception) =>
            Log(category, indentLevel, exception, message: null, args: null);
        public Events.MessageLoggedEventArgs Log(LogCategory category, string message, params object[] args) =>
            Log(category, 0, default, message, args);
        public Events.MessageLoggedEventArgs Log(LogCategory category, int indentLevel, string message, params object[] args) =>
            Log(category, indentLevel, default, message, args);
        public Events.MessageLoggedEventArgs Log(LogCategory category, Exception exception, string message, params object[] args) =>
            Log(category, 0, exception, message, args);
        public Events.MessageLoggedEventArgs Log(LogCategory category, int indentLevel, Exception exception, string message, params object[] args) =>
            Events.Post(new Events.MessageLoggedEventArgs(this, category, indentLevel, exception, message, args));

        protected internal virtual void OnMessageLogged(Events.MessageLoggedEventArgs e) => Events.RaiseEvent(e);

        protected internal virtual void OnSslCertificateSelecting(Events.SslCertificateSelectingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnSslCertificateValidating(Events.SslCertificateValidatingEventArgs e) => Events.RaiseEvent(e);

        protected internal virtual void OnCommandExecuting(Events.CommandExecutingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnCommandExecuteCompleted(Events.CommandExecuteCompletedEventArgs e) => Events.RaiseEvent(e);

        protected internal virtual void OnDisconnecting(Events.DisconnectingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnConnectCompleted(Events.ConnectCompletedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnDisconnectCompleted(Events.DisconnectCompletedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnConnectProgressChanged(Events.ConnectProgressChangedEventArgs e) => Events.RaiseEvent(e);

        protected internal virtual void OnPacketSendCompleted(Events.PacketSendCompletedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnPacketReceiveCompleted(Events.PacketReceiveCompletedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnPacketSendProgressChanged(Events.PacketSendProgressChangedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnPacketReceiveProgressChanged(Events.PacketReceiveProgressChangedEventArgs e) => Events.RaiseEvent(e);

        public static void Run(Node node, Func<Task> func)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var prevCtx = SynchronizationContext.Current;

            try

            {


                var syncCtx = new SynchronizationContext();

                SynchronizationContext.SetSynchronizationContext(syncCtx);


                var dh = new Func<Task<Result>>(async () =>
                {
                    return await node.ConnectAsync();
                });

                var dy = node.ConnectAsync();

                var t = func();

                //dy.ContinueWith(delegate { syncCtx.Complete(); }, TaskScheduler.Default);



                //syncCtx.RunOnCurrentThread();



                t.GetAwaiter().GetResult();

            }

            finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
        }

        //Result EnsureUniqueProcess()
        //{
        //    var result = Result.Success;

        //    if (Config.ExclusiveProcess && !AppMode)
        //    {
        //        Mutex = new Mutex(true, Config.ExclusiveProcessName, out var createdNew);

        //        if (!createdNew)
        //        {
        //            Mutex.Dispose();
        //            Mutex = null;
        //            var sc = IsServer ? nameof(Server).ToLowerInvariant() : nameof(Client).ToLowerInvariant();
        //            var message = $"An instance of '{Config.ExclusiveProcessName}' {sc} is already running. " +
        //                $"You can only run one instance of this {sc} process on the same machine.";

        //            Log(LogCategory.Warning, message);

        //            result = new Result(ResultCode.Error, message);
        //        }
        //    }

        //    return result;
        //}

#if NET35
        internal Task<Result> LoadPluginsAsync()
            => Utilities.Task.FromResult(Result.Success);
#else
        internal async Task<Result> LoadPluginsAsync()
        {
            var result = Result.Success;

            if (!Config.Plugins.Enabled)
                Log(LogCategory.Warning, 0, "Plugins disabled");
            else
            {
                Log(LogCategory.OperationHeader, 0, "Loading plugins...");

                var assemblies = Config.Plugins.Assemblies as IEnumerable<Assembly>;
                var loadAssemblies = new HashSet<Assembly>();

                var so = Directory.GetCurrentDirectory();

                if (!Directory.Exists(Config.Plugins.Path))
                {
                    var errorMessage = $"Can't find plugins path '{Config.Plugins.Path}'";
                    result = new Result(ResultCode.Error, errorMessage);

                    Log(LogCategory.Warning, 0, errorMessage);
                    Log(LogCategory.SuccessHeader, 0, "Done loading plugins.");

                    return result;
                }
#if !NETSTANDARD1_3

                if ((assemblies?.Count() ?? 0) == 0)
                    assemblies = Directory.EnumerateFiles(Config.Plugins.Path, "*.dll")
                        .Select(p => Assembly.LoadFrom(p));
#endif
                foreach (var assembly in assemblies)
                    if (!LoadedPluginAssemblies.Contains(assembly))
//#if !NETSTANDARD1_3
//                    if (!(Config.EntryAssembly?.GetReferencedAssemblies()?.Contains(assembly.GetName()) ?? false))
//#endif
                    {
                        loadAssemblies.Add(assembly);
                        LoadedPluginAssemblies.Add(assembly);
                        //Log(LogCategory.Operation, 1, $"Reading file [{assembly.FullName}]");
                    }
#if NET40
                var catalog = new AggregateCatalog();

                foreach (var assembly in loadAssemblies)
                    catalog.Catalogs.Add(new AssemblyCatalog(assembly));

                var container = new CompositionContainer(catalog);
                container.ComposeParts(this);
#else
                new ContainerConfiguration().WithAssemblies(loadAssemblies).CreateContainer().SatisfyImports(this);
#endif
                foreach (var plugin in Plugins)
                {
                    await plugin.InitializeAsync(this);
                    Log(LogCategory.Operation, 1, $"Plugin loaded [{plugin.Assembly.FullName}]");
                }
            }

            Log(LogCategory.Success, 0, "Done loading plugins.");
            return result;
        }
#endif

        public Task<Result> ConnectAsync()
            => ConnectAsync(Timeout.Infinite, CancellationToken.None);

        public Task<Result> ConnectAsync(TimeSpan timeout)
            => ConnectAsync(timeout, CancellationToken.None);

        public Task<Result> ConnectAsync(int millisecondsTimeout)
            => ConnectAsync(millisecondsTimeout, CancellationToken.None);

        public Task<Result> ConnectAsync(CancellationToken cancellationToken)
            => ConnectAsync(Timeout.Infinite, cancellationToken);

        public Task<Result> ConnectAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var totalMilliseconds = (long)timeout.TotalMilliseconds;

            if (totalMilliseconds < -1 || totalMilliseconds > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            return ConnectAsync((int)totalMilliseconds, cancellationToken);
        }

        public Task<Result> ConnectAsync(int millisecondsTimeout, CancellationToken cancellationToken)
            => LinkageConnectAsync(millisecondsTimeout, cancellationToken);

        async Task<Result> LinkageConnectAsync(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            var result = Result.Success;

            if (!Linkage.ConnectionAcquire())
                return result = new Result(ResultCode.NodeConnectionFailed);

            try
            {
                Config.ProcessConfigFile();

                //if (!(result = EnsureUniqueProcess()))
                //    return result;

                if (!(result = await LoadPluginsAsync()))
                    return result;

                ConnectionScope = CreateScope(resetServiceProvider: true);

                result = await ProtectedConnectAsync(millisecondsTimeout, cancellationToken);
            }
            catch (Exception ex)
            {
                result = new Result(exception: ex);
            }
            finally
            {
                Linkage.ConnectionRelease(result);
            }

            return result;
        }

        protected virtual async Task<Result> ProtectedConnectAsync(int millisecondsTimeout, CancellationToken cancellationToken, [CallerMemberName] string callerMethodName = default)
        {
            var il = callerMethodName == nameof(LinkageConnectAsync) ? 0 : 1;

            Reset();

            var result = Result.Success;

            try
            {
#region Common

                var startTime = DateTime.Now;
                //var remainingTime = Timeout.Infinite;

                if (millisecondsTimeout < 0 && millisecondsTimeout != Timeout.Infinite)
                    throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));

                Log(LogCategory.OperationHeader, il, "Initializing network...");

                Events.Post(new Events.ConnectProgressChangedEventArgs(this, 0, 5, "Initializing network..."));

                var GetRemainingTime = new Func<int>(() => millisecondsTimeout == Timeout.Infinite ? Timeout.Infinite :
                    millisecondsTimeout - (int)(DateTime.Now - startTime).TotalMilliseconds);

                Process.GetCurrentProcess().PriorityClass = Config.ProcessPriorityClass;
                Log(LogCategory.Information, 1 + il, $"Process priority class set to {Config.ProcessPriorityClass}");

                Events.Post(new Events.ConnectProgressChangedEventArgs(this, 1, 10, "Validating configuration..."));

                if (!(result = Config.Validate()))
                    return result;

                if (!(result = NetworkInformation.Validate()))
                    return result;

                var actualPort = AsClient?.Config?.Proxy?.Enabled ?? false ? AsClient.Config.Proxy.Port : Config.Port;
                var actualAddress = AsClient?.Config?.Proxy?.Enabled ?? false ? AsClient.Config.Proxy.Address : Config.Address;
                actualAddress = !string.IsNullOrEmpty(actualAddress) ? actualAddress : !Config.PreferIPv4Addresses && Socket.OSSupportsIPv6 ? IPAddress.IPv6Any.ToString() : IPAddress.Any.ToString();

                if (!IPAddress.TryParse(actualAddress, out var ipAddress))
                {
                    try
                    {
                        if (actualAddress != IPAddress.Any.ToString() && actualAddress != IPAddress.IPv6Any.ToString())
                            IPHostEntry = await Utilities.Dns.GetHostEntryAsync(actualAddress).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        IPHostEntry = null;
                        result = new Result(ResultCode.NetworkAddressInvalid, exception: ex);
                    }

                    if (IPHostEntry == null)
                    {
                        if (string.Compare(actualAddress, "localhost", ignoreCase: true) != 0)
                            return result;

                        ipAddress = Config.PreferIPv4Addresses ? IPAddress.Loopback : IPAddress.IPv6Loopback;
                    }
                    else
                    {
                        var bestLevel = 0;
                        var preferredLocalIPv4Address = NodeNetworkInformation.PreferredLocalIPv4Address;
                        var preferredLocalIPv6Address = NodeNetworkInformation.PreferredLocalIPv6Address;

                        void SelectAddress(IPAddress address, int level)
                        {
                            if (bestLevel < level)
                            {
                                bestLevel = level;
                                ipAddress = address;
                            }
                            else if (bestLevel == level)
                                if (address.ToString() == (address.AddressFamily == AddressFamily.InterNetworkV6 ? preferredLocalIPv6Address : preferredLocalIPv4Address)?.ToString())
                                    ipAddress = address;
                        }

                        foreach (var address in IPHostEntry.AddressList)
                        {
                            if (!Config.PreferIPv4Addresses && Socket.OSSupportsIPv6 && address.AddressFamily == AddressFamily.InterNetworkV6 && !IPAddress.IsLoopback(address))
                                SelectAddress(address, 5);
                            else if (address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(address))
                                SelectAddress(address, 4);
                            else if (!Config.PreferIPv4Addresses && Socket.OSSupportsIPv6 && address.AddressFamily == AddressFamily.InterNetworkV6)
                                SelectAddress(address, 3);
                            else if (address.AddressFamily == AddressFamily.InterNetwork)
                                SelectAddress(address, 2);
                            else
                                SelectAddress(address, 1);
                        }
                    }
                }

                if (!result && IPHostEntry == null)
                {
                    Log(LogCategory.Warning, result.Exception, "Error when resolving the remote address hostname ({0}). "
                        + "The .NET Runtime is '{1}'.", actualAddress, Utilities.Platform.NETRuntime);

                    result = Result.Success;
                }

                if (IsClient)
                {
                    AsClient.RemoteEndPoint = new IPEndPoint(ipAddress, actualPort);
                }
                else
                {
                    LocalEndPoint = new IPEndPoint(ipAddress, actualPort);

                    //var localIpAddress = (Config.PreferIPv4Addresses && Socket.OSSupportsIPv6) ? IPAddress.IPv6Any : IPAddress.Any;
                    //LocalEndPoint = new IPEndPoint(localIpAddress, Config.Port);
                }

                if (this is Client)
                    AsClient.Link = Pools.PopLink();

                Socket CreateSocket(AddressFamily family, SocketType socketType)
                {
                    switch (socketType)
                    {
                        case SocketType.Dgram: return new Socket(family, SocketType.Dgram, ProtocolType.Udp);
                        case SocketType.Stream: return new Socket(family, SocketType.Stream, ProtocolType.Tcp);

                        default: throw new Exception(Utilities.ResourceStrings.CyxorInternalException);
                    }

                    //if (socket == TcpSocket)
                    //{
                    //    var client = this as Client;

                    //    if (client == null)
                    //        socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //    else
                    //        client.Link.TcpSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //}
                    //else
                    //    socket = new Socket(ipAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                }

                TcpSocket = CreateSocket(ipAddress.AddressFamily, SocketType.Stream);

                if (Config.UdpEnabled)
                    UdpSocket = CreateSocket(ipAddress.AddressFamily, SocketType.Dgram);

                Events.Post(new Events.ConnectProgressChangedEventArgs(this, 1, 20, "Creating sockets..."));

#endregion Common

#region Server
                if (IsServer)
                {
                    Events.Post(new Events.ConnectProgressChangedEventArgs(this, 1, 90, "Initializing network..."));

                    if (AsServer.LocalEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                        if (Socket.OSSupportsIPv6)
                        {
                            Utilities.Socket.SetDualMode(TcpSocket, AsServer.Config.DualModeSocket);

                            if (Config.UdpEnabled)
                                Utilities.Socket.SetDualMode(UdpSocket, AsServer.Config.DualModeSocket);
                        }

                    TcpSocket.Bind(LocalEndPoint);
                    TcpSocket.Listen(AsServer.Config.Backlog);

                    if (Config.UdpEnabled)
                        UdpSocket.Bind(LocalEndPoint);

                    //var AcceptAsync = default(Action<SocketAsyncEventArgs>);

                    //var AcceptCompleted = new EventHandler<SocketAsyncEventArgs>(async (sender, saea) =>
                    async void AcceptCompleted(object sender, SocketAsyncEventArgs saea)
                    {
                        var link = saea.UserToken as Link;

                        if (saea.SocketError != SocketError.Success)
                        {
                            if (saea.SocketError == SocketError.OperationAborted)
                            {
                                if (link.Connection.State != ConnectionState.Connecting)
                                    Log(LogCategory.Fatal, "Invalid client state.");
                                else
                                {
                                    link.Connection.State = ConnectionState.Disconnecting;
                                    Pools.Push(link);
                                }

                                return;
                            }
                            else
                                Log(LogCategory.Fatal, "Unhandled socket error.");

                            return;
                        }

                        await link.ConnectAsync(saea.AcceptSocket);

                        AcceptAsync(saea);

#if !DEBUG
                        //Utilities.Task.Run(async () =>
                        //{
                        //    var task = Utilities.Task.Delay(3000, link.LoginTimeoutCts.Token);
                        //    await task.ConfigureAwait(false);

                        //    if (task.IsCanceled)
                        //        return;

                        //    if (server.Connections.AcceptedLinks.TryRemove(link.AddressHash, out link))
                        //        if (link.Connection.State != ConnectionState.Registering)
                        //            //Node.DisconnectClient(link, new Result(ResultCode.ClientLoginTimeout);
                        //            link.Disconnect(new Result(ResultCode.ClientLoginTimeout));
                        //});
#endif
                    }

                    //AcceptAsync = new Action<SocketAsyncEventArgs>(saea =>
                    void AcceptAsync(SocketAsyncEventArgs saea)
                    {
                        var link = Pools.PopLink();
                        link.Connection.State = ConnectionState.Connecting;

                        if (saea.AcceptSocket != null)
                            if (!IsUnityEditor)
                                saea.AcceptSocket = null; // NOTE: This may cause Unity Editor hang when playing the scene for second time!
                            else
                            {
                                saea.Completed -= AcceptCompleted;
                                //saea.Dispose(); // NOTE: This may cause Unity Editor hang when playing the scene for second time!
                                saea = new SocketAsyncEventArgs();
                                saea.Completed += AcceptCompleted;
                            }

                        saea.UserToken = link;

                        if (!TcpSocket.AcceptAsync(saea))
                            AcceptCompleted(saea.AcceptSocket, saea);
                    }

                    Parallel.For(0, AsServer.Config.MaxConcurrentAcceptTasks, i =>
                    {
                        var saea = new SocketAsyncEventArgs();
                        saea.Completed += AcceptCompleted; // TODO: Cleanup
                        AcceptAsync(saea);
                    });

                    AsServer.IsBound = result;

                    return result;
                }
#endregion Server
#region Client
                else
                {
                    // TODO: Restore this and fix
                    //client.Link.Reset(); // TODO: Move this to the disconnect or to the finalizer

                    var connection = new Awaitable();

                    var saea = new SocketAsyncEventArgs();

                    var ConnectAsync = new EventHandler<SocketAsyncEventArgs>(delegate
                    {
                        if (saea.SocketError != SocketError.Success)
                            result = new Result(ResultCode.SocketError, saea.SocketError.ToString());

                        connection.TrySetResult(result);
                    });

                    saea.Completed += ConnectAsync;
                    saea.RemoteEndPoint = AsClient.RemoteEndPoint;

                    TcpSocket.NoDelay = AsClient.Config.TcpNoDelay;

                    AsClient.Connection.State = ConnectionState.Connecting;

                    connection.Action = () =>
                    {
                        if (!TcpSocket.ConnectAsync(saea))
                            ConnectAsync(TcpSocket, saea);
                    };

                    if (!await connection.ConfigureAwait(false))
                        return connection.GetResult();

                    saea.Completed -= ConnectAsync;
                    saea.Dispose();

                    if (AsClient.Config.Proxy.Enabled)
                        if (!(result = await AsClient.ProxyConnectAsync()))
                            return result;

                    if (!(result = await AsClient.Link.ConnectAsync(TcpSocket)))
                        return result;

                    if (Config.AuthenticationMode != AuthenticationSchema.None)
                        return await AsClient.AuthenticateAsync(GetRemainingTime(), cancellationToken);

                    return result;
                }
#endregion Client
            }
#region Final
            catch (Exception ex)
            {
                if (result.Exception != null)
                    ex = new AggregateException(Utilities.ResourceStrings.AggregateException, result.Exception, ex);

                return result = new Result(ResultCode.Exception, exception: ex);
            }
            finally
            {
                try
                {
                    var nodeName = nameof(Client);

                    if (IsServer)
                    {
                        nodeName = nameof(Server);

                        if (!result)
                        {
                            TcpSocket?.Dispose();


                            // TODO: Disconnect the database
                            // TODO: Disconnect the server
                        }

                        // TODO: Create a clean function for when the node disconnects by error or whatever everythinh gets cleaned!
                        if (UdpSocket != null) // TODO: Move this to the disconnect or to the finalizer
                        {
                            UdpSocket.Dispose();
                            UdpSocket = null;
                        }
                    }
                    else
                    {
                        AsClient.Connection.Result = result;

                        if (!result)
                        {
                            if (TcpSocket.Connected)
                                TcpSocket.Dispose();

                            //if (AsClient.Connection.State != ConnectionState.Disconnected || AsClient.Connection.State != ConnectionState.Disconnecting)
                            //    await AsClient.Link.DisconnectAsync(result, ShutdownSequence.Abortive, DisconnectionSource.Local);

                            //if ((bool)TcpSocket?.Connected)
                            //    TcpSocket.Dispose();

                            //client.Connection.State = ConnectionState.Disconnected;

                            //client.Link.Reset();
                        }
                    }

                    if (result)
                    {
                        Pools.Initialize();
                        //Config.ReconnectEnabled = true;
                        Statistics.ConnectionDate = DateTime.Now;
                        Events.Post(new Events.ConnectProgressChangedEventArgs(this, 0, 100, nodeName + " connected."));
                    }

                    Events.Post(new Events.ConnectCompletedEventArgs(this, result));
                }
                catch (Exception ex)
                {
                    if (result.Exception != null)
                        ex = new AggregateException(Utilities.ResourceStrings.AggregateException, result.Exception, ex);

                    result = new Result(ResultCode.Exception, exception: ex);
                }

                if (result)
                    Log(LogCategory.Success, il, "Network successfully initialized");
                else
                {
                    TrySetResult(result);
                    Log(LogCategory.ErrorHeader, il, "Network initialization failed");
                }
            }
#endregion Final
        }

        public Task<Result> DisconnectAsync()
            => DisconnectAsync(Result.Success, ShutdownSequence.Graceful);

        public Task<Result> DisconnectAsync(Result reason)
            => DisconnectAsync(reason, ShutdownSequence.Graceful);

        public Task<Result> DisconnectAsync(ShutdownSequence shutdownSequence)
            => DisconnectAsync(Result.Success, shutdownSequence);

        public Task<Result> DisconnectAsync(Result reason, ShutdownSequence shutdownSequence)
            => DisconnectAsync(reason, shutdownSequence, DisconnectionSource.None);

        public Task<Result> DisconnectAsync(Result reason, ShutdownSequence shutdownSequence, DisconnectionSource disconnectionSource)
            => LinkageDisconnectAsync(reason, shutdownSequence, disconnectionSource);

        async Task<Result> LinkageDisconnectAsync(Result reason, ShutdownSequence shutdownSequence, DisconnectionSource disconnectionSource)
        {
            var result = Result.Success;

            if (!Linkage.DisconnectionAcquire())
                return result = new Result(ResultCode.NodeDisconnectionFailed);

            try
            {
                result = await ProtectedDisconnectAsync(reason, shutdownSequence, disconnectionSource);
            }
            finally
            {
                Linkage.DisconnectionRelease(true);
            }

            return result;
        }

        protected virtual async Task<Result> ProtectedDisconnectAsync(Result reason, ShutdownSequence shutdownSequence, DisconnectionSource disconnectionSource, [CallerMemberName] string callerMethodName = default)
        {
            var result = Result.Success;

            try
            {
                if (IsClient)
                {
                    await AsClient.Link.DisconnectAsync(reason, shutdownSequence, disconnectionSource).ConfigureAwait(false);
                    return result = AsClient.Connection.Result;
                }

                AsServer.TcpSocket.Dispose();
                AsServer.TcpSocket = null;

                var shutdownMode = shutdownSequence == ShutdownSequence.Graceful ? "gracefully" : "abortively";
                DisconnectReason = reason.Comment ?? $"The server process was {shutdownMode} shutdown.";

                if (shutdownSequence == ShutdownSequence.Graceful)
                    await Events.Post(new Events.DisconnectingEventArgs(this, DisconnectReason)).ConfigureAwait(false);

                AsServer.Connections.Links.AsParallel().ForAll(async (link) => await link.DisconnectAsync(new Result(comment: DisconnectReason), shutdownSequence, DisconnectionSource.Local).ConfigureAwait(false));

                // TODO: Verify connections are clear

                //if (shutdownSequence == ShutdownSequence.Abortive)
                //    Pools.Reset();

                return result;

            }
            catch (Exception ex)
            {
                result = new Result(ResultCode.Exception, exception: ex);
            }
            finally
            {
                try
                {
                    if (IsClient)
                    {
                        //if (!result)
                        //    client.Link.Reset();

                        //if (client.IsConnected || true)
                        //    Log(LogCategory.Fatal, "Client still connected after disconnection.");
                    }

                    //var nodeName = nameof(Client);

                    if (IsServer)
                    {
                        try
                        {
                            //DisconnectListener();

                            // TODO: Review
                            //server.Connections.ConnectedLinks.Clear();

                            Events.Post(new Events.DisconnectCompletedEventArgs(this, result.Comment));
                        }
                        catch (Exception ex)
                        {
                            if (result.Exception != null)
                                ex = new AggregateException("Review the inner exceptions for details.", result.Exception, ex);

                            result = new Result(ResultCode.Exception, exception: ex);
                        }
                    }

                    Events.Post(new Events.DisconnectCompletedEventArgs(this, result.Comment));
                }
                catch (Exception ex)
                {
                    if (result.Exception != null)
                        ex = new AggregateException("Review the inner exceptions for details.", result.Exception, ex);

                    result = new Result(ResultCode.Exception, exception: ex);
                }

                Pools.Reset();

                Config.ReconnectInterval = Config.ReconnectInterval;

                TrySetResult(result);
            }

            return result;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
