// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System;
using System.Text;
using System.Reflection;
using System.Composition;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Alimatic.Datadin.Produccion
{
    using Geia.Data;
    using Minal.Data;

    using Cyxor.Networking;

    [Export(typeof(IPlugin))]
    class Network : IPlugin
    {
        public static Master server;
        public static Master Server => LazyInstance.Value;

        static Lazy<Master> LazyInstance = new Lazy<Master>(() => server ?? new Master());

        public Assembly Assembly => typeof(Network).Assembly;

        public Task InitializeAsync(Node node)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            server = node as Master;

            server.Config.Services.AddDbContext<GeiaDbContext>(p
                => p.UseMySql(server.Config.Database.Engine.GetConnectionString("DatadinProduccionGeia")),
                    contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);

            server.Config.Services.AddDbContext<MinalDbContext>(p
                => p.UseMySql(server.Config.Database.Engine.GetConnectionString("DatadinProduccionMinal")),
                    contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);

            server.Controllers.Register(Assembly);

            return Task.CompletedTask;
        }
    }
}
// { Alimatic.Datadin } - Backend
