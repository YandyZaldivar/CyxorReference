// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Datadin.Produccion.Minal.Data
{
#if NETSTANDARD
    using Microsoft.EntityFrameworkCore.Design;

    public class MinalDbContextFactory : IDesignTimeDbContextFactory<MinalDbContext>
    {
        public MinalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MinalDbContext>();
            optionsBuilder.UseMySql(Network.Server.Config.Database.Engine.GetConnectionString("DatadinProduccionMinal"));
            return new MinalDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class MinalDbContextFactory : IDbContextFactory<MinalDbContext>
    {
        public MinalDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MinalDbContext>();
            optionsBuilder.UseMySql(Network.Server.Config.Database.Engine.GetConnectionString("DatadinProduccionMinal"));
            return new MinalDbContext(optionsBuilder.Options);
        }
    }
#endif
}
// { Alimatic.Datadin } - Backend
