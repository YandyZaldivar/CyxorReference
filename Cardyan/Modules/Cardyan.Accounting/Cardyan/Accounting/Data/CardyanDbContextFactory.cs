using Cardyan.Accounting.Data;
using Microsoft.EntityFrameworkCore;

namespace Cardyan.Inventory.Data
{
#if NETSTANDARD
    using Microsoft.EntityFrameworkCore.Design;

    public class CardyanDbContextFactory : IDesignTimeDbContextFactory<CardyanDbContext>
    {
        public CardyanDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CardyanDbContext>();
            optionsBuilder.UseMySql(MySqlConnection.String);
            return new CardyanDbContext(optionsBuilder.Options);
        }
    }
#else
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public class CardyanDbContextFactory : IDbContextFactory<CardyanDbContext>
    {
        public CardyanDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CardyanDbContext>();
            optionsBuilder.UseMySql(MySqlConnection.String);
            return new CardyanDbContext(optionsBuilder.Options);
        }
    }
#endif

    static class MySqlConnection
    {
        internal static string String
            => "user=root;password=cyxor;server=localhost;port=12484;database=Cardyan";
    }
}
