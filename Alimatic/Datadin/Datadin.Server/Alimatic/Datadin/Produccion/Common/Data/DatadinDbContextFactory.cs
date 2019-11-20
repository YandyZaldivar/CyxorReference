// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Datadin.Produccion.Data
{
#if NETSTANDARD
    using Microsoft.EntityFrameworkCore.Design;

    public class DatadinDbContextFactory : IDesignTimeDbContextFactory<DatadinDbContext>
    {
        public DatadinDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatadinDbContext>();
            optionsBuilder.UseMySql(Network.Server.Config.Database.Engine.GetConnectionString("DatadinProduccion"));
            return new DatadinDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class DatadinDbContextFactory : IDbContextFactory<DatadinDbContext>
    {
        public DatadinDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatadinDbContext>();
            optionsBuilder.UseMySql(Network.Server.Config.Database.Engine.GetConnectionString("DatadinProduccion"));
            return new DatadinDbContext(optionsBuilder.Options);
        }
    }
#endif
}
// { Alimatic.Datadin } - Backend
