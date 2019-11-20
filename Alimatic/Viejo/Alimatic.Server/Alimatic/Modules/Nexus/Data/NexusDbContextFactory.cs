/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Data
{
#if NETCOREAPP2_1 || NET461
    using Microsoft.EntityFrameworkCore.Design;

    public class NexusDbContextFactory : IDesignTimeDbContextFactory<NexusDbContext>
    {
        public NexusDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NexusDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Nexus)));
            return new NexusDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class NexusDbContextFactory : IDbContextFactory<NexusDbContext>
    {
        public NexusDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NexusDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Nexus)));
            return new NexusDbContext(optionsBuilder.Options);
        }
    }
#endif
}
/* { Alimatic.Server } */
