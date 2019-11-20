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
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Cyxor
{
    using Networking;
    using static Networking.Utilities.Threading;

    public class PostgreSqlServerManager : Master.MasterDatabase.EngineManager
    {
        //static Process InitDb;
        //static Process Pg_Ctl;

        //static bool Done = false;

        const string PwFile = "pwfile";
        const string DataDir = "pgsql/data";
        const string PostgresLog = "postgres.log";
        const string InitDbLog = "postgres_initdb.log";

        const string InitDbPath = "pgsql/bin/initdb";
        const string Pg_CtlPath = "pgsql/bin/pg_ctl";

        //static bool InitializeDatabase = false;

        public PostgreSqlServerManager(Master master) : base(master)
        {

        }

        protected internal override async Task<Result> ConnectAsync()
        {
            var result = Result.Success;

            if (!Directory.Exists(DataDir))
            {
                //Directory.CreateDirectory(DataDir);

                var initDbAwaitable = new Awaitable();

                Console.WriteLine("PostgreSQL database cluster missing.");
                Console.WriteLine("Creating a new PostgreSQL database cluster...");

                File.WriteAllText(PwFile, nameof(Cyxor).ToLower(), encoding: Encoding.UTF8);

                var InitDb = new Process
                {
                    EnableRaisingEvents = true,

                    StartInfo = new ProcessStartInfo
                    {
                        FileName = InitDbPath,
                        Arguments = $"-A trust -E utf8 -U ufo --pwfile={PwFile} -D {DataDir}"
                    }
                };

                InitDb.Exited += (s, e) =>
                {
                    if (InitDb.ExitCode == 0)
                        Console.WriteLine("PostgreSQL database cluster created successfully.");
                    else
                    {
                        Console.WriteLine("PostgreSQL database cluster creation failed:");
                        // TODO: Load log from file.
                    }

                    File.Delete(PwFile);

                    initDbAwaitable.TrySetResult(Result.Success);
                };

                InitDb.Start();

                await initDbAwaitable.ConfigureAwait(false);
            }

            Console.WriteLine("Starting PostgreSQL server...");

            var pg_ctlAwaitable = new Awaitable();

            var Pg_Ctl = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Pg_CtlPath,
                    Arguments = $"start -w -l {PostgresLog} -D {DataDir} -o \"-e\""
                },
            };

            Pg_Ctl.Exited += (s, e) =>
            {
                if (Pg_Ctl.ExitCode == 0)
                    Console.WriteLine("PostgreSQL server started successfully.");
                else
                {
                    Console.WriteLine("PostgreSQL server starting failed:");
                    //Console.WriteLine(File.ReadAllText(PostgresLog));
                }

                pg_ctlAwaitable.TrySetResult(Result.Success);
            };

            Pg_Ctl.EnableRaisingEvents = true;
            Pg_Ctl.Start();

            await pg_ctlAwaitable.ConfigureAwait(false);




            //using (var cyxorDbContext = new CyxorDbContext())
            //{
            //    //await cyxorDbContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            //    await cyxorDbContext.Database.EnsureDeletedAsync().ConfigureAwait(false);
            //    await cyxorDbContext.Database.MigrateAsync().ConfigureAwait(false);
            //    Console.WriteLine("Cyxor.db created.");
            //}

            try
            {

                //Console.WriteLine("Initializing UFOServer database.");

                //using (var ufoDbContext = new UfoDbContext())
                //{
                //    // Workaround.1 to possible pgsql EF bug.
                //    try
                //    {
                //        await ufoDbContext.Database.MigrateAsync().ConfigureAwait(false);
                //    }
                //    catch
                //    {
                //        Console.WriteLine("Trying workaround.1");
                //        await ufoDbContext.Database.MigrateAsync().ConfigureAwait(false);
                //    }

                //    Console.WriteLine("Ufo.db created.");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine();
            Console.Write($"{nameof(Cyxor)}>");

            /*
            var server = new Server();

            server.Events.MessageLogged += (s, e) =>
                Console.WriteLine($"Log '{e.Category}': {e.Message}");

            server.Events.PacketReceiveCompleted += (s, e) =>
                Console.WriteLine($"Message with id '{e.Packet.Id}' received: {e.Packet.GetMessage()}");

            await server.ConnectAsync().ConfigureAwait(false);
            Console.WriteLine("Server started.");

            Console.WriteLine();
            Console.WriteLine("Press a key to exit...");
            Console.WriteLine();

            */

            return result;
        }

        protected internal override async Task<Result> DisconnectAsync()
        {
            var result = Result.Success;

            Console.WriteLine("Shutting down PostgreSQL server...");

            var pg_ctlAwaitable = new Awaitable();

            var Pg_Ctl = new Process
            {
                EnableRaisingEvents = true,

                StartInfo = new ProcessStartInfo
                {
                    FileName = Pg_CtlPath,
                    Arguments = $"stop -D {DataDir} -m f"
                }
            };

            Pg_Ctl.Exited += delegate { pg_ctlAwaitable.TrySetResult(Result.Success); };

            Pg_Ctl.Start();

            await pg_ctlAwaitable.ConfigureAwait(false);

            if (Pg_Ctl.ExitCode == 0)
                Console.WriteLine("PostgreSQL server stopped successfully.");
            else
            {
                Console.WriteLine("PostgreSQL server shut down failed:");
                //Console.WriteLine(File.ReadAllText(PostgresLog));
            }

            //Done = true;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Exiting process...");

            await Task.Delay(5000).ConfigureAwait(false);

            //Environment.Exit(0);

            return result;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
