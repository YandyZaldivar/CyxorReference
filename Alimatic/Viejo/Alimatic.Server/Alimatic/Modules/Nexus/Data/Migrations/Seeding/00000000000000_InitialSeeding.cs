/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Alimatic.Nexus.Data.Migrations
{
    using Models;

    [DbContext(typeof(NexusDbContext))]
    [Migration("00000000000000_InitialSeeding")]
    //public class InitialSeeding : Migration
    public class InitialSeeding : Initial
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var scope = Network.Instance.ConnectionScope;

            try
            {
                scope = scope ?? Network.Instance.CreateScope();
                var securities = new List<Security>(Enum.GetValues(typeof(SecurityValue)).Length);
                foreach (var security in Enum.GetValues(typeof(SecurityValue)))
                    securities.Add(new Security { Id = (int)security, Value = (SecurityValue)Enum.Parse(typeof(SecurityValue), security.ToString()) });

                var permissions = new List<Permission>(Enum.GetValues(typeof(PermissionValue)).Length);
                foreach (var permission in Enum.GetValues(typeof(PermissionValue)))
                    permissions.Add(new Permission { Id = (int)permission, Value = (PermissionValue)Enum.Parse(typeof(PermissionValue), permission.ToString()) });

                var columnTypes = new List<ColumnType>(Enum.GetValues(typeof(ColumnTypeValue)).Length);
                foreach (var columnType in Enum.GetValues(typeof(ColumnTypeValue)))
                    columnTypes.Add(new ColumnType { Id = (int)columnType, Value = (ColumnTypeValue)Enum.Parse(typeof(ColumnTypeValue), columnType.ToString()) });

                var dbContext = scope.GetService<NexusDbContext>();

                dbContext.Securities.AddRange(securities);
                dbContext.Permissions.AddRange(permissions);
                dbContext.ColumnTypes.AddRange(columnTypes);

                dbContext.SaveChanges();
            }
            finally
            {
                if (scope != Network.Instance.ConnectionScope)
                    scope.Dispose();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var scope = Network.Instance.CreateScope())
            {
                var dbContext = scope.GetService<NexusDbContext>();

                dbContext.Securities.RemoveRange(dbContext.Securities);
                dbContext.Permissions.RemoveRange(dbContext.Permissions);
                dbContext.ColumnTypes.RemoveRange(dbContext.ColumnTypes);

                dbContext.SaveChanges();
            }
        }
    }
}
/* { Alimatic.Server } */
