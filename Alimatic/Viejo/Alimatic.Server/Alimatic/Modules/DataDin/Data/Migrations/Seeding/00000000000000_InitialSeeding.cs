/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

using AutoMapper;

namespace Alimatic.DataDin.Data.Migrations
{
    using Models;

    [DbContext(typeof(DataDinDbContext))]
    [Migration("00000000000000_InitialSeeding")]
    //public class InitialSeeding : Migration
    public class InitialSeeding : Initial
    {
        static Network Network => Network.Instance;

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var scope = Network.CreateScope())
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<FilaApiModel, Fila>(MemberList.None);
                    cfg.CreateMap<GrupoApiModel, Grupo>(MemberList.None);
                    cfg.CreateMap<EmpresaApiModel, Empresa>(MemberList.None);
                });

                var mapper = config.CreateMapper();

                var filas = FilaApiModel.Filas.Select(p => mapper.Map<Fila>(p));
                var grupos = GrupoApiModel.Grupos.Select(p => mapper.Map<Grupo>(p));
                var modelos = ModeloApiModel.Modelos.Select(p => mapper.Map<Modelo>(p));
                var empresas = EmpresaApiModel.Empresas.Select(p => mapper.Map<Empresa>(p));
                var divisiones = DivisionApiModel.Divisiones.Select(p => mapper.Map<Division>(p));

                //var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                //var lines = File.ReadLines($"{path}\\EstadosFinancieros.csv");

                var estadosFinancieros = new SortedSet<EstadoFinanciero>(new EstadoFinancieroComparer());

                //foreach (var line in lines)
                //{
                //    var tokens = line.Split(new char[] { ',' });

                //    var ef = new EstadoFinanciero
                //    {
                //        ModeloId = int.Parse(tokens[0]),
                //        EmpresaId = int.Parse(tokens[1]),
                //        FilaId = int.Parse(tokens[2]),
                //        C1 = decimal.Parse(tokens[3]),
                //        C2 = decimal.Parse(tokens[4]),
                //        C3 = decimal.Parse(tokens[5]),
                //        Mes = int.Parse(tokens[6]),
                //        Año = int.Parse(tokens[7]),
                //    };

                //    ef.Empresa = empresas.Single(p => p.Id == ef.EmpresaId);

                //    estadosFinancieros.Add(ef);
                //}

                var dbContext = scope.GetService<DataDinDbContext>();

                dbContext.Filas.AddRange(filas);
                dbContext.Modelos.AddRange(modelos);
                dbContext.Divisiones.AddRange(divisiones);
                dbContext.Grupos.AddRange(grupos);
                dbContext.Empresas.AddRange(empresas);
                dbContext.EstadosFinancieros.AddRange(estadosFinancieros);

                dbContext.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var scope = Network.Instance.CreateScope())
            {
                var dbContext = scope.GetService<DataDinDbContext>();

                dbContext.EstadosFinancieros.RemoveRange(dbContext.EstadosFinancieros);
                dbContext.Empresas.RemoveRange(dbContext.Empresas);
                dbContext.Grupos.RemoveRange(dbContext.Grupos);
                dbContext.Divisiones.RemoveRange(dbContext.Divisiones);
                dbContext.Modelos.RemoveRange(dbContext.Modelos);
                dbContext.Filas.RemoveRange(dbContext.Filas);

                dbContext.SaveChanges();
            }
        }
    }
}
/* { Alimatic.Server } */
