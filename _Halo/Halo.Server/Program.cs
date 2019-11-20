/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Proyecto Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Halo
{
    using Data;
    using Accounts.Data;

    using Cyxor;
    using Cyxor.Data;
    using Cyxor.Models;
    using Cyxor.Networking;
    using Cyxor.Networking.Config;
    using Cyxor.Networking.Config.Server;

    class Network : Master
    {
        public static new Network Instance => LazyInstance.Value;
        static Lazy<Network> LazyInstance = new Lazy<Network>(() => new Network());
    }

    class Program
    {
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            Network.Instance.Config.ExclusiveProcess = true;
            Network.Instance.Config.Database.Enabled = true;
            Network.Instance.Config.Name = "Halo.Server";
            //Network.Instance.Config.UserVersion = Version.Value;
            //Network.Instance.Config.AuthenticationMode = AuthenticationMode.Basic;

            Network.Instance.Config.Database.Engine.Enabled = true;
            Network.Instance.Config.Database.Engine.Provider = DatabaseEngineProvider.MySql;

            // TODO: Test creating migration for Network

            //Network.Instance.Config.Services.AddDbContext<MasterDbContext>(p => p.UseInMemoryDatabase(nameof(Cyxor)), ServiceLifetime.Scoped);

            //Network.Instance.Config.Services.AddDbContext<HaloDbContext>(p => p.UseInMemoryDatabase(nameof(Halo)), ServiceLifetime.Scoped);

            Network.Instance.Config.Services.AddDbContext<HaloDbContext>(contextLifetime: ServiceLifetime.Scoped,
                optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Halo))));

            Network.Instance.Config.Services.AddDbContext<AccountsDbContext>(contextLifetime: ServiceLifetime.Scoped,
                optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Accounts))));

            //App.Run(Network.Instance, "Halo", args: args);
            App.Run(Network.Instance, args: args);

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey(intercept: true);
        }
    }
}
/* { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
