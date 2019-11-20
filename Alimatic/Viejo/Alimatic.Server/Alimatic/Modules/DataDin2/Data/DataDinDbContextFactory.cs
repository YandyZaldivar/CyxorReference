/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;

namespace Alimatic.DataDin2.Data
{
#if NETCOREAPP2_1 || NET461
    using Microsoft.EntityFrameworkCore.Design;

    public class DataDin2DbContextFactory : IDesignTimeDbContextFactory<DataDin2DbContext>
    {
        public DataDin2DbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataDin2DbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(DataDin2)));
            return new DataDin2DbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class DataDin2DbContextFactory : IDbContextFactory<DataDin2DbContext>
    {
        public DataDin2DbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataDin2DbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(DataDin2));
            return new DataDin2DbContext(optionsBuilder.Options);
        }
    }
#endif
}
/* { Alimatic.Server } */
