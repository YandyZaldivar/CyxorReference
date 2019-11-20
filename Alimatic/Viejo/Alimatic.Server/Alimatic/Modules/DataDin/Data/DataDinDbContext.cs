/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.DataDin.Data
{
    using Models;

    public class DataDinDbContext : DbContext
    {
        public DbSet<Fila> Filas { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Division> Divisiones { get; set; }
        public DbSet<EstadoFinanciero> EstadosFinancieros { get; set; }

        public DataDinDbContext() { }

        public DataDinDbContext(DbContextOptions<DataDinDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grupo>().HasKey(p => new { p.DivisionId, p.Id });
            modelBuilder.Entity<Division>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Fila>().HasKey(p => new { p.Id, p.ModeloId });
            modelBuilder.Entity<Empresa>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Empresa>().HasIndex(p => p.NombreCompleto).IsUnique();
            modelBuilder.Entity<EstadoFinanciero>().HasKey(p => new { p.Año, p.Mes, p.EmpresaId, p.ModeloId, p.FilaId });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                property.Relational().ColumnType = $"{nameof(Decimal)}({22}, {2})";

            base.OnModelCreating(modelBuilder);
        }
    }
}
/* { Alimatic.Server } */
