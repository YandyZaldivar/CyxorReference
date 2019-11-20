/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Halo.Data
{
    //public class HaloDbContextFactory : IDbContextFactory<HaloDbContext>
    public class HaloDbContextFactory : IDesignTimeDbContextFactory<HaloDbContext>
    {
        //public HaloDbContext Create(DbContextFactoryOptions options)
        public HaloDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HaloDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Halo)));
            return new HaloDbContext(optionsBuilder.Options);
        }
    }
}
/* { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
