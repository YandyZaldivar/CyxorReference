/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;


namespace Alimatic.Accounts.Data
{
#if NETCOREAPP2_1 || NET461
    using Microsoft.EntityFrameworkCore.Design;

    public class AccountsDbContextFactory : IDesignTimeDbContextFactory<AccountsDbContext>
    {
        public AccountsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccountsDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Accounts)));
            return new AccountsDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class AccountsDbContextFactory : IDbContextFactory<AccountsDbContext>
    {
        public AccountsDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccountsDbContext>();
            optionsBuilder.UseMySql(Network.Instance.Config.Database.Engine.GetConnectionString(nameof(Accounts)));
            return new AccountsDbContext(optionsBuilder.Options);
        }
    }
#endif
}
/* { Alimatic.Server } */
