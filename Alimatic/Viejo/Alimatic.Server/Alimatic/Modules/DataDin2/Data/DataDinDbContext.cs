/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.DataDin2.Data
{
    using Models;

    public class DataDin2DbContext : DbContext
    {
        public DbSet<Row> Rows { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }

        public DataDin2DbContext() { }

        public DataDin2DbContext(DbContextOptions<DataDin2DbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<UserRole>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<UserModel>().HasKey(p => new { p.UserId, p.ModelId });
            modelBuilder.Entity<Group>().HasKey(p => new { p.DivisionId, p.Id });
            modelBuilder.Entity<Division>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Row>().HasKey(p => new { p.Id, p.ModelId });
            modelBuilder.Entity<Enterprise>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Enterprise>().HasIndex(p => p.FullName).IsUnique();
            modelBuilder.Entity<Template>().HasKey(p => new { p.Year, p.Month, p.Day, p.ModelId });
            modelBuilder.Entity<Record>().HasKey(p => new { p.Year, p.Month, p.Day, p.EnterpriseId, p.ModelId, p.RowId });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                property.Relational().ColumnType = $"{nameof(Decimal)}({22}, {2})";

            base.OnModelCreating(modelBuilder);
        }
    }
}
/* { Alimatic.Server } */
