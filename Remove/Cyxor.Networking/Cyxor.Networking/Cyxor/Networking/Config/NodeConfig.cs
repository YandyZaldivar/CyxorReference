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
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Net.Sockets;
using System.Collections;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config
{
    using Extensions;
    using Serialization;
    using Utilities = Utilities;

    [JsonObject]
    public abstract class NodeConfig
    {
        protected bool IsReadOnly { get; set; }
        //public static readonly NodeConfig Default;

        protected const string ConnectedReadOnlyExceptionMsg = "Cannot change properties while this network instance is active.";

//#if !NET35
        //[XmlIgnore]
        //[CyxorIgnore]
        //[JsonIgnore]
        //ConfigJsonSerializer JsonSerializer { get; set; }

        public virtual string SerializeToXml() =>
            JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(this), GetType().Name, writeArrayAttribute: true).ToString();

        public virtual void DeserializeFromXml(string text) =>
            DeserializeFromJson(JsonConvert.SerializeXNode(XDocument.Parse(text), Formatting.None, omitRootObject: true).ToString());

        #region Version1
        //public virtual string SerializeToJson() => SerializeToJson(excludeDefaultValues: false);
        //public virtual string SerializeToJson(bool excludeDefaultValues) => (JsonSerializer ?? (JsonSerializer = new ConfigJsonSerializer(this))).Serialize(excludeDefaultValues);

        //public virtual void DeserializeFromJson(string text) => (JsonSerializer ?? (JsonSerializer = new ConfigJsonSerializer(this))).Load(text);

        //public virtual void Save() => Save(excludeDefaultValues: false);
        //public virtual void Save(bool excludeDefaultValues) => (JsonSerializer ?? (JsonSerializer = new ConfigJsonSerializer(this))).Save(excludeDefaultValues);

        //public virtual void Load() => (JsonSerializer ?? (JsonSerializer = new ConfigJsonSerializer(this))).Load();
        //public virtual void Load(string jsonText) => (JsonSerializer ?? (JsonSerializer = new ConfigJsonSerializer(this))).Load(jsonText);
        #endregion


        #region Version2
        //public virtual string SerializeToJson() => SerializeToJson(excludeDefaultValues: false);
        //public virtual string SerializeToJson(bool excludeDefaultValues) => JsonCommentSerializer.Instance.Serialize(this, excludeDefaultValues);

        //public virtual void DeserializeFromJson(string text) => JsonCommentSerializer.Instance.Load(text, this);

        //public virtual void Save() => Save(excludeDefaultValues: false);
        //public virtual void Save(bool excludeDefaultValues) => JsonCommentSerializer.Instance.Save(File.Name, this, excludeDefaultValues);

        //public virtual void Load() => JsonCommentSerializer.Instance.LoadFromFile(File.Name, this);
        //public virtual void Load(string jsonText) => JsonCommentSerializer.Instance.Load(jsonText, this);
        #endregion

        public virtual string SerializeToJson()
            => SerializeToJson(includeComments: true, excludeDefaultValues: false);
        public virtual string SerializeToJson(bool includeComments, bool excludeDefaultValues)
            => Utilities.Json.Serialize(this, includeComments: includeComments, excludeDefaultValues: excludeDefaultValues);

        public virtual void DeserializeFromJson(string value) => Utilities.Json.Populate(value, this);

        public virtual void Save() => Save(includeComments: true, excludeDefaultValues: false);
        public virtual void Save(bool includeComments, bool excludeDefaultValues)
            => System.IO.File.WriteAllText(File.Name, SerializeToJson(includeComments, excludeDefaultValues));

        public virtual void Load(string value) => DeserializeFromJson(value);
        public virtual void Load() => DeserializeFromJson(System.IO.File.ReadAllText(File.Name));

        //#endif

        protected NodeConfig(bool readOnly) : this()
        {
            IsReadOnly = readOnly;
        }

        public NodeConfig()
        {
            //SerialProtocol = SerialProtocol.Xml;

            //foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            //{
            //	DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];
            //	if (attr != null)
            //	{
            //		prop.SetValue(this, attr.Value);
            //	}
            //}

            //var assembly = typeof(NodeConfig).GetTypeInfo().Assembly;
            //var assemblyName = assembly.GetName().Name;
            //var assemblyLocation = assembly.Location == "" ? @".\" : assembly.Location;
            //var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            //receivedFilesDirectory = assemblyDirectory + @"\ReceivedFiles";
            //FileName = string.Concat(assemblyDirectory, @"\", assemblyName, ".conf");

            ReconnectTimer = new Timer(async (sender) =>
            {
                if (!ReconnectEnabled)
                    return;

                try
                {
                    if (Node.IsConnected)
                        return;

                    await Node.ConnectAsync().ConfigureAwait(false);
                }
                catch
                {
                    // TODO: Log
                }
                finally
                {
                    if (ReconnectEnabled)
                        ReconnectTimer.Change(ReconnectInterval, ReconnectInterval);
                }
            }, null, Timeout.Infinite, Timeout.Infinite);

            var address = Socket.OSSupportsIPv6 ? IPAddress.IPv6Any : IPAddress.Any;

            //if (Utilities.Platform.IsWindowsNT)
            //{
            //    var key = @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\TCPIP\Parameters";

            //    tcpWindowSize = (int)Microsoft.Win32.Registry.GetValue(key, "TcpWindowSize", tcpWindowSize);
            //    tcpTimedWaitDelaySeconds = (int)Microsoft.Win32.Registry.GetValue(key, "TcpTimedWaitDelay", tcpTimedWaitDelaySeconds);
            //}

            maxTCPReceiveBufferSize = tcpWindowSize;
        }

        [CyxorIgnore]
        Node node;
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Node Node
        {
            get => node;
            internal set => node = value;
        }

        protected internal virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null) =>
            SetPropertyInternal(ref property, value, GetType(), propertyName: propertyName);

        internal void SetPropertyInternal<T>(ref T property, T value, Type type, string propertyName = null)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("The default root configuration instance cannot be modified.");

            var isNotNullDefined = false;
            var isConnectedModifiableDefined = false;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.Name == propertyName);

            foreach (var propertyItem in properties)
            {
                isNotNullDefined |= propertyItem.IsDefined(typeof(NotNullAttribute), inherit: true);
                isConnectedModifiableDefined |= propertyItem.IsDefined(typeof(ConnectedModifiableAttribute), inherit: true);
            }

            if (value == null && isNotNullDefined)
                throw new ArgumentNullException(nameof(value), string.Format(Utilities.ResourceStrings.ExceptionFormat, type.Name, propertyName));

            if (typeof(T).GetTypeInfo().IsEnum)
                if (!Enum.IsDefined(typeof(T), value))
                    throw new ArgumentException(string.Format("Undefined enumeration value in {0}.", typeof(T).Name));

            if (Node != null)
                if (Node.IsConnected && !isConnectedModifiableDefined)
                    throw new InvalidOperationException(ConnectedReadOnlyExceptionMsg);

            property = value;

//#pragma warning disable IDE0019 // Use pattern matching
//            var configProperty = property as ConfigProperty;
//#pragma warning restore IDE0019 // Use pattern matching

//            if (configProperty != null)
//            {
//                if (configProperty.RootConfig == null)
//                {
//                    configProperty.RootConfig = this;
//                    configProperty.Initialize();
//                }
//            }

            if (property is ConfigProperty configProperty)
            {
                if (configProperty.RootConfig == null)
                {
                    configProperty.RootConfig = this;
                    configProperty.Initialize();
                }
            }
        }

        [CyxorIgnore]
        SynchronizationContext synchronizationContext;
        [CyxorIgnore]
        protected internal int SynchronizationContextManagedThreadId { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("TODO:")]
        public SynchronizationContext SynchronizationContext
        {
            get => synchronizationContext;
            set
            {
                var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                var propertyInfo = value.GetType().GetProperty("DestinationThread", bindingFlags);
                var thread = propertyInfo.GetGetMethod(nonPublic: true).Invoke(value, parameters: null);
                SynchronizationContextManagedThreadId = (thread as Thread).ManagedThreadId;
                SetProperty(ref synchronizationContext, value);
            }
        }

        [CyxorIgnore]
        Assembly entryAssembly
#if NETSTANDARD1_3
            = default;
#else
            = Assembly.GetEntryAssembly();
#endif
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("TODO:")]
        public Assembly EntryAssembly
        {
            get => entryAssembly;
            set => SetProperty(ref entryAssembly, value);
        }

        public const string DefaultName = default;
        //internal string name = DefaultName;
        string name = DefaultName;
        public string Name
        {
            get => name;
            set =>SetProperty(ref name, value);
        }

        /// <summary>
        /// This method allows to change the name internally without validating the connection state.
        /// This is useful after authentication when the Server sends the properly validated name.
        /// Normally properties can't be setted while the network instance is connected.
        /// </summary>
        /// <param name="value"></param>
        internal void SetName(string value) => name = value;

        public const string DefaultCommandBackingSerializerName = "Json";
        string commandBackingSerializerName = DefaultCommandBackingSerializerName;
        [ConnectedModifiable(true)]
        [DefaultValue(DefaultCommandBackingSerializerName)]
        public string CommandBackingSerializerName
        {
            get => commandBackingSerializerName;
            set => SetProperty(ref commandBackingSerializerName, value);
        }

        [JsonIgnore]
        public IBackingSerializer CommandBackingSerializer =>
            CommandBackingSerializerName == "Json" ? (IBackingSerializer)JsonBackingSerializer.Instance : XmlBackingSerializer.Instance;

        //public const AssemblyBuilder DefaultAssemblyBuilder = null;
        //[CyxorIgnore]
        //AssemblyBuilder assemblyBuilder = DefaultAssemblyBuilder;
        //[Description("TODO:")]
        //[DefaultValue(DefaultAssemblyBuilder)]
        //[JsonIgnore]
        //public virtual AssemblyBuilder AssemblyBuilder
        //{
        //    get => assemblyBuilder;
        //    set => SetProperty(ref assemblyBuilder, value);
        //}

        public const AuthenticationSchema DefaultAuthenticationMode = AuthenticationSchema.None;
        AuthenticationSchema authenticationMode = DefaultAuthenticationMode;
        [Description("TODO:")]
        [DefaultValue(DefaultAuthenticationMode)]
        public virtual AuthenticationSchema AuthenticationMode
        {
            get => authenticationMode;
            set => SetProperty(ref authenticationMode, value);
        }

        SrpConfig srp;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public SrpConfig Srp
        {
            get => srp ?? (Srp = new SrpConfig());
            set => SetProperty(ref srp, value);
        }

        SslConfig ssl;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public SslConfig Ssl
        {
            get => ssl ?? (Ssl = new SslConfig());
            set => SetProperty(ref ssl, value);
        }

        FileConfig file;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public FileConfig File
        {
            get => file ?? (File = new FileConfig());
            set => SetProperty(ref file, value);
        }

        internal void ProcessConfigFile()
        {
            try
            {
                //var fileName = Node.Config.File.Name == FileConfig.DefaultName ?
                //    Name ?? FileConfig.DefaultName : Node.Config.File.Name ?? FileConfig.DefaultName;

                //if (fileName == Name)
                //    fileName += $"{nameof(Config)}.json";

                switch (Node.Config.File.Mode)
                {
                    case FileConfigMode.None: break;
                    case FileConfigMode.Create: Node.Config.Save(); break;
                    case FileConfigMode.Open:
                        {
                            if (System.IO.File.Exists(Node.Config.File.Name))
                                Node.Config.Load();

                            break;
                        }
                    case FileConfigMode.OpenOrCreate:
                        {
                            if (System.IO.File.Exists(Node.Config.File.Name))
                                Node.Config.Load();
                            else
                                Node.Config.Save();

                            break;
                        }
                }
            }
            catch// (Exception ex)
            {
                //Console.WriteLine(exc.ToString());
            }
        }

        LogConfig fileLog;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public LogConfig FileLog
        {
            get => fileLog ?? (FileLog = new LogConfig());
            set => SetProperty(ref fileLog, value);
        }

        NameConfig names;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public NameConfig Names
        {
            get => names ?? (Names = new NameConfig());
            set => SetProperty(ref names, value);
        }

        PluginConfig plugins;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public PluginConfig Plugins
        {
            get => plugins ?? (Plugins = new PluginConfig());
            set => SetProperty(ref plugins, value);
        }

        PacketConfig packets;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public PacketConfig Packets
        {
            get => packets ?? (Packets = new PacketConfig());
            set => SetProperty(ref packets, value);
        }

        ConsoleConfig console;
        [Description("TODO:")]
        [DefaultValue(null)]
        [Category(nameof(ConfigProperty))]
        public ConsoleConfig Console
        {
            get => console ?? (Console = new ConsoleConfig());
            set => SetProperty(ref console, value);
        }

        [ReadOnly(true)]
        [Description("Cyxor version.")]
        [DefaultValue("0.1.0-alpha-1")]
        public string CyxorVersion
        {
            get => typeof(Node).GetTypeInfo().Assembly.GetName().Version.ToString();
            set { }
        }

        string userVersion = "1.0.0-*";
        [ReadOnly(true)]
        [Description("User provided version.")]
        [DefaultValue("1.0.0-*")]
        public string UserVersion
        {
            get => userVersion;
            set => userVersion = value;
        }

        bool encryptionEnabled = false;
        [DefaultValue(false)]
        [Description("Specifies whether or not to encrypt internal data in secure communications. " +
           "This value has no effect if the server side is not using a database for Cyxor integrated security.")]
        public bool EncryptionEnabled
        {
            get => encryptionEnabled;
            set => SetProperty(ref encryptionEnabled, value);
        }

        public const bool DefaultExclusiveProcess = false;
        bool exclusiveProcess = DefaultExclusiveProcess;
        [DefaultValue(DefaultExclusiveProcess)]
        [Description("TODO:")]
        public bool ExclusiveProcess
        {
            get => exclusiveProcess;
            set => SetProperty(ref exclusiveProcess, value);
        }

        public const string DefaultExclusiveProcessName = DefaultName;
        string exclusiveProcessName = DefaultExclusiveProcessName;
        [DefaultValue(DefaultExclusiveProcessName)]
        [Description("TODO:")]
        public string ExclusiveProcessName
        {
            get => exclusiveProcessName ?? (exclusiveProcessName = Name);
            set => SetProperty(ref exclusiveProcessName, value);
        }

        int transferReportPercentThreshold = 1;
        [DefaultValue(1)]
        [Description("The threshold percentage for data transfer progress updates.")]
        public int TransferReportPercentThreshold
        {
            get => transferReportPercentThreshold;
            set => transferReportPercentThreshold = value;
        }

        int transferReportMillisecondsThreshold = 1000;
        [DefaultValue(1000)]
        [Description("The threshold milliseconds for data transfer progress updates.")]
        public int TransferReportMillisecondsThreshold
        {
            get => transferReportMillisecondsThreshold;
            set => transferReportMillisecondsThreshold = value;
        }

        int queryMillisecondsTimeout = Timeout.Infinite; // TODO: set to a reasonable value
        [DefaultValue(5000)]
        [Description("TODO:")]
        public int QueryMillisecondsTimeout
        {
            get => queryMillisecondsTimeout;
            set => SetProperty(ref queryMillisecondsTimeout, value);
        }

        string receivedFilesDirectory;
        [Description("TODO:")]
        //[EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ReceivedFilesDirectory
        {
            get => receivedFilesDirectory;
            set => SetProperty(ref receivedFilesDirectory, value);
        }

        bool udpEnabled;
        [DefaultValue(false)]
        [Description("TODO:")]
        public bool UdpEnabled
        {
            get => udpEnabled;
            set => SetProperty(ref udpEnabled, value);
        }

        bool wsEnabled;
        [DefaultValue(false)]
        [Description("TODO:")]
        public bool WebSocketsEnabled
        {
            get => wsEnabled;
            set => SetProperty(ref wsEnabled, value);
        }

        bool overrideEvents;
        [DefaultValue(false)]
        [Description("TODO:")]
        public bool OverrideEvents
        {
            get => overrideEvents;
            set => SetProperty(ref overrideEvents, value);
        }

        EventDispatching eventDispatching = EventDispatching.Sequenced;
        [DefaultValue(EventDispatching.Sequenced)]
        [Description("Controls how network events are presented. Useful for increase performance or simplify coding.")]
        public EventDispatching EventDispatching
        {
            get => eventDispatching;
            set => SetProperty(ref eventDispatching, value);
        }

        public const ProcessPriorityClass DefaultProcessPriorityClass = ProcessPriorityClass.Normal;
        ProcessPriorityClass processPriorityClass = DefaultProcessPriorityClass;
        [DefaultValue(DefaultProcessPriorityClass)]
        [Description("TODO:")]
        public ProcessPriorityClass ProcessPriorityClass
        {
            get => processPriorityClass;
            set => SetProperty(ref processPriorityClass, value);
        }

        int ioBufferSize = 8192;
        [DefaultValue(8192)]
        [Description("TODO:")]
        public int IOBufferSize
        {
            get => ioBufferSize;
            set => SetProperty(ref ioBufferSize, value);
        }

        int maxTCPReceiveBufferSize = 64240;
        [DefaultValue(64240)]
        [Description("TODO:")]
        public int MaxTCPReceiveBufferSize
        {
            get => maxTCPReceiveBufferSize;
            set => SetProperty(ref maxTCPReceiveBufferSize, value);
        }

        int tcpWindowSize = 64240;
        [DefaultValue(64240)]
        [Description("TODO:")]
        public int TcpWindowSize => tcpWindowSize;

        public const string DefaultAddress = "localhost";
        string address = DefaultAddress;
        [Description("TODO:")]
        public virtual string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public const bool DefaultPreferIPv4Addresses = false;
        bool preferIPv4Addresses = DefaultPreferIPv4Addresses;
        [DefaultValue(DefaultPreferIPv4Addresses)]
        [Description("TODO:")]
        public bool PreferIPv4Addresses
        {
            get => preferIPv4Addresses;
            set => SetProperty(ref preferIPv4Addresses, value);
        }

        public const int DefaultPort = 29530;
        int port = DefaultPort;
        [DefaultValue(DefaultPort)]
        [Description("TODO:")]
        public int Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        bool appAutoStart = default;
        [DefaultValue(false)]
        [Description("TODO:")]
        public bool AppAutoStart
        {
            get => appAutoStart;
            set => SetProperty(ref appAutoStart, value);
        }

        [CyxorIgnore]
        Timer ReconnectTimer;

        bool reconnectEnabled = default;
        [DefaultValue(false)]
        [Description("TODO:")]
        [ConnectedModifiable(enabled: true)]
        public bool ReconnectEnabled
        {
            get => reconnectEnabled;
            set
            {
                SetProperty(ref reconnectEnabled, value);

                if (reconnectEnabled)
                    ReconnectTimer.Change(reconnectInterval, reconnectInterval);
                else
                    ReconnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        int reconnectInterval = Timeout.Infinite;
        [DefaultValue(Timeout.Infinite)]
        [Description("TODO:")]
        public int ReconnectInterval
        {
            get => reconnectInterval;
            set
            {
                SetProperty(ref reconnectInterval, value);

                if (ReconnectEnabled)
                    ReconnectTimer.Change(reconnectInterval, reconnectInterval);
            }
        }

        //IEnumerable services;
        //[Description("TODO:")]
        //[JsonIgnore]
        //[CyxorIgnore]
        //public virtual IEnumerable Services
        //{
        //    get => services;
        //    set => SetProperty(ref services, value);
        //}

        //IRazorViewStringRenderer razorViewStringRenderer;
        //[Description("TODO:")]
        //[JsonIgnore]
        //[CyxorIgnore]
        //public virtual IServiceCollection Services
        //{
        //    get => services ?? (services = new ServiceCollection());
        //    set => SetProperty(ref services, value);
        //}

        public virtual Result Validate()
        {
            var result = Result.Success;

            try
            {
                // TODO: fix for NET4.0
                SerializeToJson();
            }
            catch (Exception exc)
            {
                return result = new Result(ResultCode.Exception, exception: exc);
            }

            if (EventDispatching == EventDispatching.Synchronized)
            {
                if (SynchronizationContext == null)
                    if ((SynchronizationContext = SynchronizationContext.Current) == null)
                        return new Result(ResultCode.SynchronizationContextNull);
            }

            //result = Names.Validate(ref name);

            return result;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
