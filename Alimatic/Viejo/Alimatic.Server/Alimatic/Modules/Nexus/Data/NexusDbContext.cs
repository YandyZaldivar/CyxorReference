/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Data
{
    using Models;

    public class NexusDbContext : DbContext
    {
        public DbSet<Row> Rows { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<RowColumn> RowColumns { get; set; }
        public DbSet<TableRole> TableRoles { get; set; }
        public DbSet<ColumnRole> ColumnRoles { get; set; }
        public DbSet<ColumnType> ColumnTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public NexusDbContext() { }

        public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Table>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Security>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Permission>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<ColumnType>().HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<RowColumn>().HasKey(p => new { p.RowId, p.ColumnId });

            modelBuilder.Entity<UserRole>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<TableRole>().HasKey(p => new { p.TableId, p.RoleId });
            modelBuilder.Entity<ColumnRole>().HasKey(p => new { p.ColumnId, p.RoleId });

            modelBuilder.Entity<Column>().HasIndex(p => new { p.Name, p.TableId }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
/* { Alimatic.Server } */
