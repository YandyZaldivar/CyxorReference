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

//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;

//#if !POMELO
//using MySQL.Data.EntityFrameworkCore.Extensions;
//#endif

namespace Cardyan
{
    //using Cardyan.Accounts.Data;
    //using Cardyan.Inventory.Data;

    using Cyxor.Networking;
    using Cyxor.Networking.Config.Server;

    public class Program
    {
        public static void Main(string[] args)
        {
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
            Network.Instance.Config.Name = $"Cardyan.Server";
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


            //Network.Instance.Config.Services.AddDbContext<AccountsDbContext>(contextLifetime: ServiceLifetime.Scoped,
            //    optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Accounts))));

            //Network.Instance.Config.Services.AddDbContext<CardyanDbContext>(contextLifetime: ServiceLifetime.Scoped,
            //                optionsAction: p => p.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Cardyan))));




            App.HeaderShown += (s, e) =>
            {
                for (var i = 0; i < Network.Instance.Config.Console.MargenLength; i++)
                    Console.Write(' ');

                Console.WriteLine("Copyright (C) 2018 Cardyan");

                for (var i = 0; i < Network.Instance.Config.Console.MargenLength; i++)
                    Console.Write(' ');

                Console.WriteLine("Servidor Cardyan. Ver 'cardyan license'");
            };

            //Network.Instance.Controllers.Register(typeof(CardyanDbContext).Assembly);

            App.Run(Network.Instance, name: $"{nameof(Cardyan)}.{nameof(Server)}", args: args);
        }
    }
}
/* { Alimatic.Server } */
