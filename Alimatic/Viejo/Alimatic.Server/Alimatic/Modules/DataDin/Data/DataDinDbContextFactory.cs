/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;

namespace Alimatic.DataDin.Data
{
#if NETCOREAPP2_1 || NET461
    using Microsoft.EntityFrameworkCore.Design;

    public class DataDinDbContextFactory : IDesignTimeDbContextFactory<DataDinDbContext>
    {
        public DataDinDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataDinDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(DataDin)));
            return new DataDinDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class DataDinDbContextFactory : IDbContextFactory<DataDinDbContext>
    {
        public DataDinDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataDinDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(DataDin)));
            return new DataDinDbContext(optionsBuilder.Options);
        }
    }
#endif
}
/* { Alimatic.Server } */
