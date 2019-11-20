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
//using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;

namespace Cyxor.Networking
{
    using Config.Server;

    public partial class Server : Node
    {
        internal override void Internal() => throw new InvalidOperationException();

        internal bool IsBound = false;

        public static new Server Instance => LazyInstance.Value;
        static Lazy<Server> LazyInstance = new Lazy<Server>(() => new Server());

        public ServerConnections Connections { get; set; }

        public new ServerEvents Events
        {
            get => base.Events as ServerEvents;
            protected set => base.Events = value;
        }

        public new ServerControllers Controllers
        {
            get => base.Controllers as ServerControllers;
            protected set => base.Controllers = value;
        }

        public new ServerStatistics Statistics
        {
            get => base.Statistics as ServerStatistics;
            private set => base.Statistics = value;
        }

        protected internal new ServerMiddleware Middleware
        {
            get => base.Middleware as ServerMiddleware;
            protected set => base.Middleware = value;
        }

        public new ServerNetworkInformation NetworkInformation
        {
            get => base.NetworkInformation as ServerNetworkInformation;
            private set => base.NetworkInformation = value;
        }

        public new ServerConfig Config
        {
            get => base.Config as ServerConfig;
            set => base.Config = value;
        }

#if UAP10_0
        public override bool IsConnected => IsBound;
#else
        public override bool IsConnected => TcpSocket != null ? TcpSocket.IsBound : false;
#endif

        public override string ToString() => string.Format(Utilities.ResourceStrings.ServerDebuggerDisplayFormat, IsConnected, Connections.Count, Config.Address, Config.Port);

        public Server() => Initialize(root: true);

        protected Server(bool root) => Initialize(root);

        void Initialize(bool root)
        {
            Config = new ServerConfig();
            Events = new ServerEvents(this);
            Statistics = new ServerStatistics();
            Middleware = new ServerMiddleware(this);
            Connections = new ServerConnections(this);
            NetworkInformation = new ServerNetworkInformation(this);

            if (root)
                Controllers = new ServerControllers(this);
        }

        //public virtual string GenerateModels()
        //{

        //}

//        ModuleBuilder PopulateModelsModule(ModuleBuilder moduleBuilder)
//        {
//            var typeBuilder = moduleBuilder.DefineType("NS.CC", TypeAttributes.Public);

//            var ctorInfo = typeof(Models.ModelAttribute).GetConstructor(new Type[] { typeof(object) });
//            var attrBuilder = new CustomAttributeBuilder(ctorInfo, new object[] { "test test" });

//            typeBuilder.SetCustomAttribute(attrBuilder);

//            var propertyName = "Name";
//            var propertyType = typeof(string);
//            var fieldName = $"m_{propertyName}";

//            var fieldBuilder = typeBuilder.DefineField(
//                fieldName,
//                propertyType,
//                FieldAttributes.Private);

//            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

//            var getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

//            var propertyGetAccessor = typeBuilder.DefineMethod(
//                $"get_{propertyName}",
//                getSetAttr,
//                propertyType,
//                Type.EmptyTypes);

//            var propertySetAccessor = typeBuilder.DefineMethod(
//                $"set_{propertyName}",
//                getSetAttr,
//                null,
//                new Type[] { propertyType });

//            var propertyGetIL = propertyGetAccessor.GetILGenerator();
//            {
//                propertyGetIL.Emit(OpCodes.Ldarg_0);
//                propertyGetIL.Emit(OpCodes.Ldfld, fieldBuilder);
//                propertyGetIL.Emit(OpCodes.Ret);
//            }

//            var propertySetIL = propertySetAccessor.GetILGenerator();
//            {
//                propertySetIL.Emit(OpCodes.Ldarg_0);
//                propertySetIL.Emit(OpCodes.Ldarg_1);
//                propertySetIL.Emit(OpCodes.Stfld, fieldBuilder);
//                propertySetIL.Emit(OpCodes.Ret);
//            }

//            propertyBuilder.SetGetMethod(propertyGetAccessor);
//            propertyBuilder.SetSetMethod(propertySetAccessor);

//#if NETSTANDARD1_3 || NETSTANDARD2_0
//            typeBuilder.CreateTypeInfo();
//#else
//            typeBuilder. CreateType();
//#endif

//            return moduleBuilder;
//        }

        protected internal virtual void OnClientRelaying(Events.Server.ClientRelayingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnClientConnected(Events.Server.ClientConnectedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnClientConnecting(Events.Server.ClientConnectingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnClientDisconnected(Events.Server.ClientDisconnectedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnClientDisconnecting(Events.Server.ClientDisconnectingEventArgs e) => Events.RaiseEvent(e);
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
