using System;
using System.Linq;
using Cardyan.Accounting.Models;
using Microsoft.EntityFrameworkCore;

namespace Cardyan.Accounting.Data
{
    //using Models;

    public class CardyanDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountEntry> AccountEntries { get; set; }
        public DbSet<AccountEntryType> AccountEntryTypes { get; set; }
        public DbSet<AccountNormalBalance> AccountNormalBalances { get; set; }
        // public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public CardyanDbContext() { }

        public CardyanDbContext(DbContextOptions<CardyanDbContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.EnableSensitiveDataLogging();
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<AccountEntryType>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<AccountNormalBalance>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<AccountEntry>().HasKey(p => new { p.AccountId, p.TransactionId });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                property.Relational().ColumnType = $"{nameof(Decimal)}({22}, {2})";

            InitialSeeding(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        void InitialSeeding(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountType>().HasData(AccountType.Items);
            modelBuilder.Entity<AccountClasification>().HasData(AccountClasification.Items);
            modelBuilder.Entity<AccountNormalBalance>().HasData(AccountNormalBalance.Items);
            modelBuilder.Entity<AccountEntryType>().HasData(AccountEntryType.Items);
        }
    }
}
