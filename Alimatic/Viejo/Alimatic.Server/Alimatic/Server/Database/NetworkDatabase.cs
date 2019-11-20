/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Alimatic
{
    using Cyxor.Networking;

    public partial class Network : Master
    {
        public class NetworkDatabase : MasterDatabase
        {
            protected Network Network;

            protected internal NetworkDatabase(Network network) : base(network)
            {
                Network = network;
            }

            //public override MasterDbContext CreateMasterDbContext() => new AccountsDbContext();
            //public virtual PtDbContext CreateFrameworkDbContext() => new PtDbContext();
            //public virtual NexusDbContext CreateNexusDbContext() => new NexusDbContext();

            //public virtual HaloDbContext CreateHaloDbContext() => new HaloDbContext();

            protected override async Task<Result> ConnectAsync()
            {
                var result = Result.Success;

                try
                {
                    if (!(result = await base.ConnectAsync().ConfigureAwait(false)))
                        return result;

                    try
                    {
                        Master.Log(LogCategory.OperationHeader, 2, "Initializing Framework database...");

                        Master.Log(LogCategory.Success, 2, "Framework database successfully initialized");
                    }
                    catch
                    {
                        Master.Log(LogCategory.ErrorHeader, 2, "Framework database initialization failed");
                        throw;
                    }

                    return result;
                }
                catch (Exception exc)
                {
                    return result = new Result(ResultCode.Exception, exception: exc);
                }
                finally
                {

                }
            }

            protected override async Task<Result> DisconnectAsync()
            {
                var result = Result.Success;

                try
                {
                    return result = await base.DisconnectAsync().ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                    return result = Result.Combine(result, new Result(ResultCode.Exception, exception: exc));
                }
                finally
                {

                }
            }
        }
    }
}
/* { Alimatic.Server } */
