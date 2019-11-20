using System;
using System.IO;
using System.Linq;
//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cardyan
{
    using Cyxor.Networking;

    class Network : Master
    {
        public static new Network Instance => LazyInstance.Value;
        static Lazy<Network> LazyInstance = new Lazy<Network>(() => new Network());

        public Network()
        {
            //var modulesPath = "Modules";

            //var fileNames = Directory.EnumerateFiles(modulesPath, "*.dll");

            //var catalog = new AggregateCatalog();
            ////catalog.Catalogs.Add(new AssemblyCatalog(typeof(CardyanDbContext).Assembly));
            //catalog.Catalogs.Add(new AssemblyCatalog(fileNames.First()));
            //var container = new CompositionContainer(catalog);


            //try
            //{
            //    container.ComposeParts(this);
            //    Plugin.Initialize(this);
            //}
            //catch (CompositionException compositionException)
            //{
            //    Console.WriteLine(compositionException.ToString());
            //}
        }
    }
}
