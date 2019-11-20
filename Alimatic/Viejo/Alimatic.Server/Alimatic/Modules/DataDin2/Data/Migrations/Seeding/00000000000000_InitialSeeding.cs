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

namespace Alimatic.DataDin2.Data.Migrations
{
    using Models;

    [DbContext(typeof(DataDin2DbContext))]
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
                    cfg.CreateMap<RowApiModel, Row>(MemberList.None);
                    cfg.CreateMap<GroupApiModel, Group>(MemberList.None);
                    cfg.CreateMap<EnterpriseApiModel, Enterprise>(MemberList.None);
                });

                var mapper = config.CreateMapper();

                var filas = RowApiModel.Rows.Select(p => mapper.Map<Row>(p));
                var grupos = GroupApiModel.Groups.Select(p => mapper.Map<Group>(p));
                var modelos = ModelApiModel.Models.Select(p => mapper.Map<Model>(p));
                var divisiones = DivisionApiModel.Divisions.Select(p => mapper.Map<Division>(p));
                var empresas = EnterpriseApiModel.Enterprises.Select(p => mapper.Map<Enterprise>(p));
                var frequencies = FrequencyApiModel.Frequencies.Select(p => mapper.Map<Frequency>(p));

                //var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                //var lines = File.ReadLines($"{path}\\EstadosFinancieros.csv");

                /*
                var records = new SortedSet<Record>(new RecordComprarer());

                foreach (var line in lines)
                {
                    var tokens = line.Split(new char[] { ',' });

                    var ef = new EstadoFinanciero
                    {
                        ModeloId = int.Parse(tokens[0]),
                        EmpresaId = int.Parse(tokens[1]),
                        FilaId = int.Parse(tokens[2]),
                        C1 = decimal.Parse(tokens[3]),
                        C2 = decimal.Parse(tokens[4]),
                        C3 = decimal.Parse(tokens[5]),
                        Mes = int.Parse(tokens[6]),
                        Año = int.Parse(tokens[7]),
                    };

                    ef.Empresa = empresas.Single(p => p.Id == ef.EmpresaId);

                    records.Add(ef);
                }
                */

                var dbContext = scope.GetService<DataDin2DbContext>();

                dbContext.Rows.AddRange(filas);
                dbContext.Groups.AddRange(grupos);
                dbContext.Models.AddRange(modelos);
                dbContext.Divisions.AddRange(divisiones);
                dbContext.Enterprises.AddRange(empresas);
                dbContext.Frequencies.AddRange(frequencies);
                //dbContext.Records.AddRange(records);

                dbContext.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var scope = Network.Instance.CreateScope())
            {
                var dbContext = scope.GetService<DataDin2DbContext>();

                //dbContext.EstadosFinancieros.RemoveRange(dbContext.EstadosFinancieros);
                dbContext.Enterprises.RemoveRange(dbContext.Enterprises);
                dbContext.Groups.RemoveRange(dbContext.Groups);
                dbContext.Divisions.RemoveRange(dbContext.Divisions);
                dbContext.Models.RemoveRange(dbContext.Models);
                dbContext.Rows.RemoveRange(dbContext.Rows);

                dbContext.SaveChanges();
            }
        }
    }
}
/* { Alimatic.Server } */
