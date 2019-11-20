/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alimatic
{
    using System.Runtime.CompilerServices;
    using Cyxor.Networking;

    public partial class Network : Master
    {
        public static new Network Instance => LazyInstance.Value;
        static Lazy<Network> LazyInstance = new Lazy<Network>(() => new Network());

        public new NetworkEvents Events
        {
            get => base.Events as NetworkEvents;
            protected set => base.Events = value;
        }

        public new NetworkControllers Controllers
        {
            get => base.Controllers as NetworkControllers;
            protected set => base.Controllers = value;
        }

        public new NetworkDatabase Database
        {
            get => base.Database as NetworkDatabase;
            protected set => base.Database = value;
        }

        public new NetworkMiddleware Middleware
        {
            get => base.Middleware as NetworkMiddleware;
            private set => base.Middleware = value;
        }

        public new NetworkConnections Connections
        {
            get => base.Connections as NetworkConnections;
            protected set => base.Connections = value;
        }

        public new NetworkConfig Config
        {
            get => base.Config as NetworkConfig;
            set => base.Config = value;
        }

        public Network() : base(root: false) => Initialize(root: true);

        protected Network(bool root) : base(root: false) => Initialize(root);

        void Initialize(bool root)
        {
            Config = new NetworkConfig();
            Events = new NetworkEvents(this);
            Database = new NetworkDatabase(this);
            Middleware = new NetworkMiddleware(this);
            Connections = new NetworkConnections(this);

            if (root)
                Controllers = new NetworkControllers(this);
        }

        public override Connection CreateConnection() => new NetworkConnection();

        protected internal virtual void OnFakeCompleted(FakeCompletedEventArgs e) => Events.RaiseEvent(e);

        protected override async Task<Result> ProtectedConnectAsync(int millisecondsTimeout, CancellationToken cancellationToken, [CallerMemberName] string callerMethodName = null)
        //protected override async Task<Result> ProtectedConnectAsync(int millisecondsTimeout, CancellationToken cancellationToken, bool linkageAcquired)
        {
            var result = Result.Success;

            try
            {
                return result = await base.ProtectedConnectAsync(millisecondsTimeout, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Result(ResultCode.Exception, exception: ex);
            }
            finally 
            {
                try
                {

                }
                catch
                {

                }
            }
        }

        protected override async Task<Result> ProtectedDisconnectAsync(Result reason, ShutdownSequence shutdownSequence, DisconnectionSource disconnectionSource, [CallerMemberName] string callerMethodName = null)
        //protected override async Task<Result> DisconnectAsync(Result reason, ShutdownSequence shutdownSequence, DisconnectionSource disconnectionSource, bool linkageAcquired)
        {
            var result = Result.Success;

            try
            {
                return result = await base.ProtectedDisconnectAsync(reason, shutdownSequence, disconnectionSource).ConfigureAwait(false);
            }
            catch
            {
                return result;
            }
            finally
            {
                try
                {

                }
                catch
                {

                }
            }
        }
    }
}
/* { Alimatic.Server } */
