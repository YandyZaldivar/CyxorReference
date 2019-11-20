using System;
using System.Reflection;
using System.Composition;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cardyan.Inventory
{
    using Data;

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
            server = node as Master;

            server.Config.Services.AddDbContext<CardyanDbContext>(p
                => p.UseMySql(server.Config.Database.Engine.GetConnectionString(nameof(Cardyan))),
                    contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);

            server.Controllers.Register(Assembly);

            return Task.CompletedTask;
        }
    }
}
