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
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace Cyxor.Networking
{
    using Data;
    using Config.Server;
    using Events.Server;
    using System.Runtime.CompilerServices;

    public partial class Master : Server
    {
        public static new Master Instance => LazyInstance.Value;
        static Lazy<Master> LazyInstance = new Lazy<Master>(() => new Master());

        public MasterDatabase Database { get; protected set; }

        public new MasterEvents Events
        {
            get => base.Events as MasterEvents;
            protected set => base.Events = value;
        }

        protected internal new MasterMiddleware Middleware
        {
            get => base.Middleware as MasterMiddleware;
            protected set => base.Middleware = value;
        }

        public new MasterControllers Controllers
        {
            get => base.Controllers as MasterControllers;
            protected set => base.Controllers = value;
        }

        public new MasterConnections Connections
        {
            get => base.Connections as MasterConnections;
            protected set => base.Connections = value;
        }

        public new MasterConfig Config
        {
            get => base.Config as MasterConfig;
            set => base.Config = value;
        }

        //protected override IServiceScope ConnectionScope
        //{
        //    protected internal get => base.ConnectionScope;
        //    protected internal set => base.ConnectionScope = value;
        //}

        //ServiceCollection InternalServices { get; set; }
        //IServiceProvider InternalServiceProvider { get; set; }

        //protected override IServiceProvider ServiceProvider
        //{
        //    get => ConfigureServices();
        //    set => base.ServiceProvider = value;
        //}

        public Master() : base(root: false) => Initialize(root: true);

        protected Master(bool root) : base(root: false) => Initialize(root);

        void Initialize(bool root)
        {
            Config = new MasterConfig();
            Events = new MasterEvents(this);
            Database = new MasterDatabase(this);
            Middleware = new MasterMiddleware(this);
            Connections = new MasterConnections(this);

            if (root)
                Controllers = new MasterControllers(this);

            //InternalServices = new ServiceCollection();
            //InternalServices.AddDbContext<MasterDbContext>(p => p.UseInMemoryDatabase(nameof(Cyxor)), ServiceLifetime.Transient);
            //InternalServices.AddDbContext<MasterDbContext>(p => p.UseInMemoryDatabase("Halo"), ServiceLifetime.Transient);
            //InternalServiceProvider = InternalServices.BuildServiceProvider();
        }

        public override Connection CreateConnection() => new MasterConnection();

        protected internal virtual void OnDbLoadCompleted(DbLoadCompletedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnDbLoadProgressChanged(DbLoadProgressChangedEventArgs e) => Events.RaiseEvent(e);

        protected internal virtual void OnAccountReset(AccountResetEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountCreated(AccountCreatedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountDeleted(AccountDeletedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountCreating(AccountCreatingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountDeleting(AccountDeletingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountResetting(AccountResettingEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountProfileUpdated(AccountProfileUpdatedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountPasswordChanged(AccountPasswordChangedEventArgs e) => Events.RaiseEvent(e);
        protected internal virtual void OnAccountSecurityChanged(AccountSecurityChangedEventArgs e) => Events.RaiseEvent(e);

        public override IServiceScope CreateScope(bool resetServiceProvider = false)
        {
            InitializeServiceProvider();
            return new ServiceScope(ServiceProvider.CreateScope(), Config.Services);

            void InitializeServiceProvider()
            {
                if (!resetServiceProvider && ServiceProvider != null)
                    return;

#if !NET451
                //ServiceProvider = Config.Services.BuildServiceProvider(validateScopes: true);
                ServiceProvider = Config.Services.BuildServiceProvider();
#else
                var extensionClassType = typeof(ServiceCollectionContainerBuilderExtensions);
                var methodName = nameof(ServiceCollectionContainerBuilderExtensions.BuildServiceProvider);
                var methodInfo = extensionClassType.GetMethod(methodName, new Type[] { typeof(IServiceCollection) });

                ServiceProvider = methodInfo.Invoke(null, new object[] { Config.Services }) as IServiceProvider;
#endif
            }
        }

        //public override object GetService(Type serviceType, bool allowSubclasses = true)
        //{
        //    var service = ServiceProvider.GetService(serviceType);

        //    if (service == null && allowSubclasses)
        //        foreach (var descriptor in Config.Services.Where(p => p.ServiceType.GetTypeInfo().IsSubclassOf(serviceType)))
        //            return ServiceProvider.GetService(descriptor.ServiceType);

        //    return service;
        //}

        //public virtual T GetService<T>(bool allowSubclasses = true) where T : class => GetService(typeof(T), allowSubclasses) as T;

        /*
        //{
            //if (!inherited)
            //    return ServiceProvider.GetService<T>();
            //else
            //{
            //    var service = default(T);

            //    foreach (var descriptor in Config.Services.Where(p => p.ServiceType.GetTypeInfo().IsSubclassOf(typeof(T))))
            //        service = ServiceProvider.GetService(descriptor.ServiceType) as T;

            //    return service;
            //}
        //}

        public virtual IEnumerable<T> GetServices<T>(bool allowSubclasses = true) where T : class
        {
            var services = new List<T>(ServiceProvider.GetServices<T>());

            if (allowSubclasses)
                foreach (var descriptor in Config.Services.Where(p => p.ServiceType.GetTypeInfo().IsSubclassOf(typeof(T))))
                    services.Add(ServiceProvider.GetService(descriptor.ServiceType) as T);

            return services;
        }
        */

        protected override async Task<Result> ProtectedConnectAsync(int millisecondsTimeout, CancellationToken cancellationToken, [CallerMemberName] string callerMethodName = null)
        {
            var result = Result.Success;

            try
            {
                Events.Post(new Events.ConnectProgressChangedEventArgs(this, 1, 30, "Loading Database..."));

                Log(LogCategory.OperationHeader, 0, "Initializing Cyxor Master...");

                if (!Config.Database.Enabled)
                    Log(LogCategory.Warning, "Database system disabled.");
                else
                {
                    Log(LogCategory.OperationHeader, 1, "Initializing database...");

                    if (!(result = await Database.ConnectAsync().ConfigureAwait(false)))
                    {
                        Log(LogCategory.ErrorHeader, 1, "Database initialization failed");
                        return result;
                    }

                    Log(LogCategory.Success, 1, "Database initialized successfully");
                }

                Events.Post(new DbLoadCompletedEventArgs(this, result));

                return result = await base.ProtectedConnectAsync(millisecondsTimeout, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                return result = Result.Combine(result, new Result(ResultCode.Exception, exception: exc));
            }
            finally
            {
                if (result)
                    Log(LogCategory.Success, 0, "Cyxor Master successfully initialized");
                else
                {
                    Log(LogCategory.OperationHeader, 1, "Shutting down database...");

                    await Database.DisconnectAsync().ConfigureAwait(false);

                    Log(LogCategory.Success, 1, "Database successfully shutdown");

                    Log(LogCategory.ErrorHeader, 0, "Cyxor Master initialization failed");
                }
            }
        }

        protected override async Task<Result> ProtectedDisconnectAsync(Result reason, ShutdownSequence shutdownSequence, DisconnectionSource disconnectionSource, [CallerMemberName] string callerMethodName = null)
        {
            var result = Result.Success;

            try
            {
                result = await base.ProtectedDisconnectAsync(reason, shutdownSequence, disconnectionSource).ConfigureAwait(false);

                result = Result.Combine(result, await Database.DisconnectAsync().ConfigureAwait(false));

                return result;
            }
            catch (Exception exc)
            {
                return result = Result.Combine(result, new Result(result, new Result(ResultCode.Exception, exception: exc)));
            }
            finally
            {

            }
        }

        protected override void OnClientConnecting(ClientConnectingEventArgs e)
        {
            //base.OnClientConnecting(e);

            e.Acquire();

            //using (var dbContext = Config.Database.CreateDbContext<DbContext>())
            //{
            //    var account = dbContext.Accounts.SingleOrDefault(acc => acc.Name == e.Connection.Name);

            //    if (e.Connection.State != ConnectionState.Authenticating)
            //        ProtocolError(e.Connection, string.Format(TextStrings.InvalidInternalOperation, e.Connection.State));
            //    else if (!link.Srp.Verify(login.Srp_M))
            //        link.Disconnect(new Result(ResultCode.AuthenticationFailed));
            //    else
            //    {
            //        login.Name = link.Connection.Name;
            //        break;
            //    }
            //}

            //if (Server.Config.Database.Enabled)
            //{
            //    loginResult.FirstLogin = account.ExpirationTime != null;

            //    if (account.Reset != null)
            //    {
            //        if (login.Reset)
            //        {
            //            loginResult.FirstLogin = true;

            //            account.SRP_s = account.Reset.SRP_s;
            //            account.SRP_v = account.Reset.SRP_v;
            //        }

            //        account.Reset = null;
            //        loginResult.ResetFlag = true;
            //    }

            //    loginResult.SrpM = clientConnectingEventArgs.Connection.Link.Srp.M;
            //}

            //if (dbContext != null)
            //{
            //    dbContext.SaveChanges();
            //    dbContext.Dispose();
            //}

            e.Release();
        }

        protected override void OnClientConnected(ClientConnectedEventArgs e)
        {
            base.OnClientConnected(e);

            //if (Server.Config.Database.Enabled)
            //{
            //    account.Connected = true;
            //    account.FailedLogins = 0;
            //    account.ExpirationTime = null;
            //    account.LastLogin = DateTime.Now;
            //    account.LastIp = link.Connection.RemoteEndPoint.Address.ToString();
            //}
        }

        //protected override async void OnClientDisconnected(ClientDisconnectedEventArgs e)
        //{
        //    // TODO: Move this to the ClientDisconnectedEventArgs action?

        //    var masterConnection = e.Connection as MasterConnection;

        //    using (var scope = CreateScope())
        //    {
        //        var dbContext = scope.GetService<MasterDbContext>();

        //        var account = await dbContext.Accounts.SingleOrDefaultAsync(p => p.Name == e.Connection.Name).ConfigureAwait(false);

        //        //account.Connected = false;

        //        dbContext.SaveChanges();
        //    }
        //}
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
