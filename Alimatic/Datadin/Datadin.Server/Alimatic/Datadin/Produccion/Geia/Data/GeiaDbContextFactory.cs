// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Datadin.Produccion.Geia.Data
{
#if NETSTANDARD
    using Microsoft.EntityFrameworkCore.Design;

    public class GeiaDbContextFactory : IDesignTimeDbContextFactory<GeiaDbContext>
    {
        public GeiaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeiaDbContext>();
            optionsBuilder.UseMySql(Network.Server.Config.Database.Engine.GetConnectionString("DatadinProduccionGeia"));
            return new GeiaDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class GeiaDbContextFactory : IDbContextFactory<GeiaDbContext>
    {
        public GeiaDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeiaDbContext>();
            optionsBuilder.UseMySql(Network.Server.Config.Database.Engine.GetConnectionString("DatadinProduccionGeia"));
            return new GeiaDbContext(optionsBuilder.Options);
        }
    }
#endif
}
// { Alimatic.Datadin } - Backend
