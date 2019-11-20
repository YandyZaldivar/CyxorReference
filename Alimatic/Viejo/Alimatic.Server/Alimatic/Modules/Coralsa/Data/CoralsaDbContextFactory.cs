/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Coralsa.Data
{
#if NETCOREAPP2_1 || NET461
    using Microsoft.EntityFrameworkCore.Design;

    public class CoralsaDbContextFactory : IDesignTimeDbContextFactory<CoralsaDbContext>
    {
        public CoralsaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoralsaDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Coralsa)));
            return new CoralsaDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class CoralsaDbContextFactory : IDbContextFactory<CoralsaDbContext>
    {
        public CoralsaDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoralsaDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Coralsa));
            return new CoralsaDbContext(optionsBuilder.Options);
        }
    }
#endif
}
/* { Alimatic.Server } */
