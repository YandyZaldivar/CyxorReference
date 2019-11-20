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
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;

#if NETSTANDARD2_0 || NET461
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
#else
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
#endif

namespace Cyxor.Networking
{
    using Data;
    using Models;
    using Config.Server;

    public partial class Master
    {
        public partial class MasterDatabase : MasterProperty
        {
            public EngineManager Engine { get; }
            public AccountManager Account { get; }

            internal ConcurrentDictionary<string, Account> EmailsPreview = new ConcurrentDictionary<string, Account>();
            internal ConcurrentDictionary<string, Account> NamesPreview = new ConcurrentDictionary<string, Account>();
            internal ConcurrentDictionary<int, Account> HashesPreview = new ConcurrentDictionary<int, Account>();

            protected internal MasterDatabase(Master master) : base(master)
            {
                Account = new AccountManager(master);

                switch (Master.Config.Database.Engine.Provider)
                {
                    case DatabaseEngineProvider.MySql: Engine = new MySqlServerManager(master); break;
                    case DatabaseEngineProvider.PostgreSql: Engine = new PostgreSqlServerManager(master); break;
                }
            }

            protected internal virtual async Task<Result> ConnectAsync()
            {
                var result = Result.Success;

                try
                {
                    ValidateServer(ignoreDisconnected: true);

                    Server.Events.Post(new Events.Server.DbLoadProgressChangedEventArgs(Node, 2, 10, ""));

                    if (!Master.Config.Database.Engine.Enabled)
                        Master.Log(LogCategory.Warning, "Database engine disabled.");
                    else
                    {
                        Master.Log(LogCategory.OperationHeader, 2, $"Initializing database engine...");

                        if (!(result = await Engine.ConnectAsync().ConfigureAwait(false)))
                        {
                            Master.Log(LogCategory.ErrorHeader, 2, "Database engine initialization failed");
                            return result;
                        }

                        Master.Log(LogCategory.Success, 2, "Database engine successfully initialized");
                    }

                    try
                    {
                        Master.Log(LogCategory.OperationHeader, 2, "Initializing Master database...");

                        foreach (var dbContext in Master.ConnectionScope.GetServices<DbContext>())
                        {
                            var genericDbContextOptionsType = typeof(DbContextOptions).GetTypeInfo().Assembly.GetTypes().
                                Single(p => p.Name == $"{nameof(DbContextOptions)}`1").MakeGenericType(dbContext.GetType());

                            // TODO: Proper filtering of no relational providers
                            var options = Master.ConnectionScope.GetService(genericDbContextOptionsType);




                            if ((options as IDbContextOptions)?.FindExtension<InMemoryOptionsExtension>() == null)
                                await dbContext.Database.MigrateAsync().ConfigureAwait(false);





                            if (dbContext is MasterDbContext masterDbContext)
                            {
                                //Account.CheckExpirationTime();

                                //·$%·$%%$/$&·

                                //var ent = from en in dbContext.Resets select en;
                                //foreach (var ee in ent)
                                //   dbContext.Resets.Remove(ee);

                                //"·$%%&%$/$%&
                            }
                        }

                        Master.Log(LogCategory.Success, 2, "Master database initialized successfully");
                    }
                    catch (Exception exc)
                    {
                        result = new Result(ResultCode.Exception, exception: exc);
                        Master.Log(LogCategory.Error, 3, message: exc.Message);
                        Master.Log(LogCategory.ErrorHeader, 2, "Master database initialization failed");
                    }
                    finally
                    {
                        
                    }

                    //Server.Events.Post(new Events.Server.DbLoadProgressChangedEventArgs(Node, 3, 60, "Loading Commands..."));


                    return result;
                }
                catch (Exception exc)
                {
                    Master.Log(LogCategory.Error, "Database loading failed.");
                    //Master.Log(LogCategory.NewLine, "");
                    //Master.Log(LogCategory.Fatal, exc.Message);
                    Master.Log(LogCategory.Fatal, exc);
                    //Master.Log(LogCategory.NewLine, "");

                    return result = new Result(ResultCode.Exception, exception: exc);
                }
                finally
                {
                    
                        //await Engine.DisconnectAsync().ConfigureAwait(false);
                }
            }

            protected internal virtual async Task<Result> DisconnectAsync()
            {
                var result = Result.Success;

                try
                {
                    Master.Log(LogCategory.OperationHeader, 2, $"Shutting down database engine...");

                    if (Master.Config.Database.Engine.Enabled)
                        return result = await Engine.DisconnectAsync().ConfigureAwait(false);

                    return result;
                }
                catch (Exception exc)
                {
                    return result = new Result(ResultCode.Exception, exception: exc);
                }
                finally
                {
                    Master.Log(LogCategory.Success, 2, "Database engine successfully shutdown");
                }
            }

            public Result ValidateServer(bool ignoreDisconnected = false)
            {
                if (!ignoreDisconnected)
                    if (!Server.IsConnected)
                        return new Result(ResultCode.NodeNotConnected);

                return Result.Success;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
