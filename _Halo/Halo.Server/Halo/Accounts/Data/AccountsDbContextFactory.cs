/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Halo.Accounts.Data
{
    //public class AccountsDbContextFactory : IDbContextFactory<AccountsDbContext>
    public class AccountsDbContextFactory : IDesignTimeDbContextFactory<AccountsDbContext>
    {
        //public AccountsDbContext CreateDbContext(DbContextFactoryOptions options)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<AccountsDbContext>();
        //    optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Accounts)));
        //    return new AccountsDbContext(optionsBuilder.Options);
        //}
        public AccountsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccountsDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Accounts)));
            return new AccountsDbContext(optionsBuilder.Options);
        }
    }
}
/* { Alimatic.Server } */
