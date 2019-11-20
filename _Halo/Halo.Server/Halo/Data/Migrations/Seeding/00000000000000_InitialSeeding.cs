/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

//using System.Reflection;
//using System.ComponentModel.DataAnnotations;

using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Halo.Data.Migrations
{
    using Data;
    using Models;

    [DbContext(typeof(HaloDbContext))]
    [Migration("00000000000000_InitialSeeding")]
    //public class InitialSeeding : Migration
    public class InitialSeeding : Initial
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var scope = Network.Instance.CreateScope())
            {
                var provincias = new Provincia[ProvinciaApiModel.Provincias.Length];

                for (var i = 0; i < ProvinciaApiModel.Provincias.Length; i++)
                    provincias[i] = new Provincia
                    {
                        Id = ProvinciaApiModel.Provincias[i].Id,
                        Nombre = ProvinciaApiModel.Provincias[i].Nombre
                    };

                var municipios = new Municipio[MunicipioApiModel.Municipios.Length];

                for (var i = 0; i < MunicipioApiModel.Municipios.Length; i++)
                    municipios[i] = new Municipio
                    {
                        Id = MunicipioApiModel.Municipios[i].Id,
                        Codigo = MunicipioApiModel.Municipios[i].Codigo,
                        Nombre = MunicipioApiModel.Municipios[i].Nombre,
                        ProvinciaId = MunicipioApiModel.Municipios[i].ProvinciaId,
                        Provincia = provincias.Single(p => p.Id == MunicipioApiModel.Municipios[i].ProvinciaId),
                    };

                foreach (var provincia in provincias)
                    foreach (var municipio in municipios)
                        if (municipio.ProvinciaId == provincia.Id)
                            provincia.Municipios.Add(municipio);

                var hospitales = new Hospital[HospitalApiModel.Hospitales.Length];

                for (var i = 0; i < HospitalApiModel.Hospitales.Length; i++)
                    hospitales[i] = new Hospital
                    {
                        Id = HospitalApiModel.Hospitales[i].Id,
                        Nombre = HospitalApiModel.Hospitales[i].Nombre,
                        ProvinciaId = HospitalApiModel.Hospitales[i].ProvinciaId,
                        Provincia = provincias.Single(p => p.Id == HospitalApiModel.Hospitales[i].ProvinciaId),
                    };

                var dbContext = scope.GetService<HaloDbContext>();

                dbContext.Provincias.AddRange(provincias);
                dbContext.Municipios.AddRange(municipios);
                dbContext.Hospitales.AddRange(hospitales);

                dbContext.Areas.AddRange(Area.List<Area>());
                dbContext.Partos.AddRange(Parto.List<Parto>());
                dbContext.Ocupaciones.AddRange(Ocupacion.List<Ocupacion>());
                dbContext.Escolaridades.AddRange(Escolaridad.List<Escolaridad>());
                dbContext.MorbilidadPartos.AddRange(MorbilidadParto.List<MorbilidadParto>());
                dbContext.IndicesMasaCorporal.AddRange(IndiceMasaCorporal.List<IndiceMasaCorporal>());
                dbContext.CausasMuerteDirecta.AddRange(CausaMuerteDirecta.List<CausaMuerteDirecta>());
                dbContext.CausasMuerteIndirecta.AddRange(CausaMuerteIndirecta.List<CausaMuerteIndirecta>());

                dbContext.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var dbContext = new HaloDbContext())
            {
                dbContext.CausasMuerteIndirecta.RemoveRange(dbContext.CausasMuerteIndirecta);
                dbContext.CausasMuerteDirecta.RemoveRange(dbContext.CausasMuerteDirecta);
                dbContext.IndicesMasaCorporal.RemoveRange(dbContext.IndicesMasaCorporal);
                dbContext.MorbilidadPartos.RemoveRange(dbContext.MorbilidadPartos);
                dbContext.Escolaridades.RemoveRange(dbContext.Escolaridades);
                dbContext.Ocupaciones.RemoveRange(dbContext.Ocupaciones);
                //dbContext.Hemorragias.RemoveRange(dbContext.Hemorragias);
                //dbContext.Ocitocicos.RemoveRange(dbContext.Ocitocicos);
                dbContext.Hospitales.RemoveRange(dbContext.Hospitales);
                dbContext.Municipios.RemoveRange(dbContext.Municipios);
                dbContext.Provincias.RemoveRange(dbContext.Provincias);
                dbContext.Partos.RemoveRange(dbContext.Partos);
                dbContext.Areas.RemoveRange(dbContext.Areas);

                dbContext.SaveChanges();
            }
        }
    }
}
/* { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
