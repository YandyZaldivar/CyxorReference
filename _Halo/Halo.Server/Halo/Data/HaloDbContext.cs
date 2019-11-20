/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using Microsoft.EntityFrameworkCore;

namespace Halo.Data
{
    using Models;

    public class HaloDbContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Hospital> Hospitales { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Ocupacion> Ocupaciones { get; set; }
        public DbSet<Escolaridad> Escolaridades { get; set; }
        public DbSet<HistorialMedico> HistorialesMedicos { get; set; }

        public DbSet<Orina> Orinas { get; set; }
        public DbSet<Condicion> Riesgos { get; set; }
        public DbSet<Hemoglobina> Hemoglobinas { get; set; }
        public DbSet<AtencionPrenatal> AtencionesPrenatales { get; set; }
        public DbSet<IndiceMasaCorporal> IndicesMasaCorporal { get; set; }
        public DbSet<UltrasonidoGenetico> UltrasonidosGeneticos { get; set; }

        public DbSet<Egreso> Egresos { get; set; }
        public DbSet<RecienNacido> RecienNacidos { get; set; }
        public DbSet<CausaMuerteDirecta> CausasMuerteDirecta { get; set; }
        public DbSet<CausaMuerteIndirecta> CausasMuerteIndirecta { get; set; }

        public DbSet<AtencionHospitalaria> AtencionesHospitalarias { get; set; }
        public DbSet<CausaMorbilidad> CausasMorbilidad { get; set; }
        public DbSet<CriterioMorbilidad> CriteriosMorbilidad { get; set; }
        public DbSet<Hemorragia> Hemorragias { get; set; }

        public DbSet<Parto> Partos { get; set; }
        public DbSet<Manejo> Manejos { get; set; }
        public DbSet<Ocitocico> Ocitocicos { get; set; }
        public DbSet<LugarIngreso> LugaresIngreso { get; set; }
        public DbSet<FallaOrganica> FallasOrganicas { get; set; }
        public DbSet<MorbilidadParto> MorbilidadPartos { get; set; }
        public DbSet<EnfermedadEspecifica> EnfermedadesEspecificas { get; set; }
        public DbSet<IntervencionQuirurgica> IntervencionesQuirurgicas { get; set; }

        public DbSet<AntecedenteGinecoObstetrico> AntecedentesGinecoObstetricos { get; set; }

        public HaloDbContext() { }

        public HaloDbContext(DbContextOptions<HaloDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provincia>().HasIndex(p => p.Nombre).IsUnique();

            //modelBuilder.Entity<Municipio>().HasKey(p => new { p.Codigo, p.ProvinciaId });

            //modelBuilder.Entity<Municipio>().HasIndex(p => new { p.Codigo, p.ProvinciaId }).IsUnique();

            //modelBuilder.Entity<Municipio>().HasAlternateKey(p => new { p.Codigo, p.ProvinciaId });
            //modelBuilder.Entity<EgresoCausaMuerte>().HasKey(p => new { p.EgresoId, p.CausaMuerteId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
/* { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
