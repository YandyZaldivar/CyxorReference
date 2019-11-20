/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Linq;
using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#if !POMELO
using MySQL.Data.EntityFrameworkCore.Extensions;
#endif

using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace Alimatic
{
    using Nexus.Data;
    using DataDin.Data;
    using DataDin2.Data;
    using Coralsa.Data;
    using Accounts.Data;

    using Cyxor.Networking;
    using Cyxor.Networking.Config.Server;

    public class Program
    {
        public static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if ((args?.Length ?? 0) == 0)
            {
                //args = new string[] { "-h", "-v" };
                //args = new string[] { "-c", $"{{ Name: '{nameof(Alimatic)}' }}" };
            }

            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider fe = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            //Pomelo.Net.Smtp.SmtpEmailSender sd = new Pomelo.Net.Smtp.SmtpEmailSender(fe, "mail.alimatic.alinet.cu", 25, "Yandy Zaldivar", "yandy@alimatic.alinet.cu", "yandy@alimatic.alinet.cu", "yandy16*");
            //sd.SendEmailAsync("yandy@alimatic.alinet.cu", "pomelo test", "este es el contenido de prueba del mensaje");

            //Network.Instance.Config.Port = 80;
            //Network.Instance.Config.Port = 9191;
            //Network.Instance.Config.Port = 29530;
            Network.Instance.Config.ExclusiveProcess = true;
            Network.Instance.Config.Database.Enabled = true;
            Network.Instance.Config.Name = $"Alimatic.Server";
            Network.Instance.Config.UserVersion = Version.Value;
            Network.Instance.Config.Database.Engine.Enabled = true;
            //Network.Instance.Config.Database.Engine.Password = null;
            //Network.Instance.Config.AuthenticationMode = AuthenticationMode.Basic;
            Network.Instance.Config.ProcessPriorityClass = ProcessPriorityClass.High;
            Network.Instance.Config.Database.Engine.Provider = DatabaseEngineProvider.MySql;
            //Network.Instance.Config.ExclusiveProcessName = $"{nameof(Alimatic)}.{nameof(Server)}";


            //Network.Instance.Config.Ssl.Enabled = true;
            Network.Instance.Config.Ssl.Password = nameof(Cyxor).ToLowerInvariant();
            Network.Instance.Config.Ssl.CertificateFileName = @"D:\Documents\Development\MakeCert\Cyxor.pfx";



            Network.Instance.Config.Services.AddDbContext<AccountsDbContext>(contextLifetime: ServiceLifetime.Scoped,
                optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Accounts))));

            //Network.Instance.Config.Services.AddDbContext<NexusDbContext>(contextLifetime: ServiceLifetime.Scoped,
            //    optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Nexus))));

            //Network.Instance.Config.Services.AddDbContext<DataDinDbContext>(contextLifetime: ServiceLifetime.Scoped,
            //    optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(DataDin))));

            Network.Instance.Config.Services.AddDbContext<DataDin2DbContext>(contextLifetime: ServiceLifetime.Scoped,
                optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(DataDin2))));

            Network.Instance.Config.Services.AddDbContext<CoralsaDbContext>(contextLifetime: ServiceLifetime.Scoped,
                optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Coralsa))));

            //Network.Instance.Config.Services.AddDbContext<CardyanDbContext>(contextLifetime: ServiceLifetime.Scoped,
            //                optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Cardyan))));

            RazorViewEngine.ConfigureServices(Network.Instance.Config.Services);






            //Network.Instance.Config.Services.AddAuthorization(p => p.AddPolicy());

            // NOTE: This was for the fix of references
            //Network.Instance.Config.Services.AddMvc()
            //    .ConfigureApplicationPartManager(manager =>
            //    {
            //        var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
            //        manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
            //        manager.FeatureProviders.Add(new ReferencesMetadataReferenceFeatureProvider());
            //    });


            //Network.Instance.Events.ClientConnected += async (s, e) =>
            //{
            //    var connection = e.Connection as MasterConnection;
            //    connection.SetNexusUser();

            //    using (var scope = Network.Instance.CreateScope())
            //    {
            //        var dbContext = scope.GetService<NexusDbContext>();
            //        var connection = e.Connection as MasterConnection;
            //        e.Connection.Tags[""] = await dbContext.Users.AsNoTracking().
            //            SingleOrDefaultAsync(p => p.AccountId == connection.Account.Id);
            //    }
            //};

            App.HeaderShown += (s, e) =>
            {
                for (var i = 0; i < Network.Instance.Config.Console.MargenLength; i++)
                    Console.Write(' ');

                Console.WriteLine("Copyright (C) 2017 Alimatic, Empresa de Sistemas Automatizados");

                for (var i = 0; i < Network.Instance.Config.Console.MargenLength; i++)
                    Console.Write(' ');

                Console.WriteLine("Servidor para la administración interna. Ver 'alimatic license'");
            };

#if NET46

            var domain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("MegaModels");
            var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);

            var name = assemblyBuilder.GetName().Name;
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name, $"{name}.dll");

            //Network.Instance.PopulateModelsModule(moduleBuilder);
            assemblyBuilder.Save($"{name}.dll");
#endif

            App.Run(Network.Instance, name: $"{nameof(Alimatic)}.{nameof(Server)}", args: args);

            //Console.WriteLine("Press a key to exit...");
            //Console.ReadKey(intercept: true);
        }
    }
}
/* { Alimatic.Server } */
